using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Fields
    //For shooting
    [SerializeField] GameObject bulletPrefab; //Prefab to be instantiated while shooting for VFX
    [SerializeField] Transform bulletPoint; //Where the bullet gets instantiated
    [SerializeField] float bulletVelocity = 3f; //Velocity of the bullet
    [SerializeField] LayerMask playerLayer; //LayerMask of the player for raycast to work
    [SerializeField] float fireRate; //Rate of fire of the gun
    [SerializeField] AudioClip gunShotSound; //Gunshot sound effect
    [SerializeField] Recoil recoilScript; //Reference to script on player that handles recoil
    [SerializeField] ParticleSystem muzzleFlash; //Prefab of particle system for the muzzle flash

    int weaponDamage = 30; //Damage of the weapon
    int weaponAmmo = 30; //Current ammo count
    const int MAX_BULLET_COUNT = 30; //Max magazine size
    int spareAmmo = 150; //Spare ammo
    bool shooting = true; //If shooting on this frame 
    float lastShootTime = 0; //Time of last shot for automatic fire 
    Coroutine reloadCoroutine; //Coroutine to allow for cancelling reloads
    //Vector3 originalGunPos;
    //Vector3 RELOAD_GUN_POS = new Vector3(-0.14f,-0.25f,-0.204f);
    Animator anim; //Player animator
    #endregion
    #region Properties
    /// <summary>
    /// Allows  UI scripts to reference weapon ammo
    /// </summary>
    public int AmmoCount
    {
        get { return weaponAmmo; }
    }

    /// <summary>
    /// Allows UI scripts to reference spare ammo
    /// </summary>
    public int SpareAmmoCount
    {
        get { return spareAmmo; }
    }
	#endregion

    /// <summary>
    /// Getting components
    /// </summary>
	void Awake()
	{
        //originalGunPos = transform.position;
        anim = GetComponent<Animator>();
	}

	/// <summary>
    /// Subsribing to event and inversing player layer for raycast to work properly
    /// </summary>
	void Start()
    {
        playerLayer = ~playerLayer;
        EventSystem.Subscribe(EventSystem.eEventType.collectPrimaryAmmo, CollectPrimaryAmmo);
        EventSystem.Subscribe(EventSystem.eEventType.adsActivated, startADSAnimation);
        EventSystem.Subscribe(EventSystem.eEventType.adsDeactivated, stopADSAnimation);
    }

    /// <summary>
    /// Checking for shooting and input
    /// </summary>
    void Update()
    {
        //Setting shooting bool
        if(Input.GetAxis("Shoot") > 0)
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
        //Shooting implementation, shoots from Camera
        if(shooting)
        {
            //When reloading cancel reload
            if (reloadCoroutine != null && weaponAmmo != 0)
            {
                //Stopping Reload
                StopCoroutine(reloadCoroutine);
                reloadCoroutine = null;
                //Stopping Audio
                AudioManager.Instance.StopGunSFX();
            }
            //Variable for ray cast data
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, playerLayer))
            {
                //Debug.Log("Hit this object: " + hit.collider.gameObject.name);
                //Enterse if enough time has passed between shots and weapon has ammo 
                if(Time.time > lastShootTime + fireRate && weaponAmmo > 0)
                {
                    //Setting new latest shot time and setting ammo
                    lastShootTime = Time.time;
                    weaponAmmo--;
                    //Audio for shooting
                    AudioSource.PlayClipAtPoint(gunShotSound, bulletPoint.position,PlayerPrefs.GetFloat("SFXVol"));
                    //Calls method for recoil
                    recoilScript.RecoilFire();
                    //Visual effect for shooting
                    if(!AimScript.IsAiming)
                    {
                        muzzleFlash.Play();
                    }
                    EventSystem.Publish(EventSystem.eEventType.shotBullet);
                    //For VFX
                    GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, transform.rotation);
                    Vector3 bulletDirection = hit.point - transform.position;
                    bulletDirection.Normalize();
                    bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletVelocity;
					//bullet.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletVelocity;

					//Handling damage from bullet				
					GameObject hitObject = hit.collider.gameObject;
					if (hitObject.tag == "Enemy")
					{
						hit.collider.gameObject.GetComponent<Enemy>().GotShot(weaponDamage);
                        Debug.Log("Did damage");
					}
                    if (hitObject.tag == "Spawner")
                    {
                        Destroy(hitObject.gameObject);
                        EventSystem.Publish(EventSystem.eEventType.enemySpawnerKilled);
                    }
				}
                //When out of ammo
                else if(Time.time > lastShootTime + (3 * fireRate) && weaponAmmo == 0)
                {
                    AudioManager.Instance.PlaySFX("DryFire");
                }
            }
        }
        //Input for reloading
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (reloadCoroutine == null)
            {
                reloadCoroutine = StartCoroutine(ReloadWeapon(2f));
            }
        }
    }

    /// <summary>
    /// Reloads the weapon after a set reload delay 
    /// This is used because no animation is currently present 
    /// Would get replaced when proper animation is implemented
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ReloadWeapon(float delay)
    {
        //Enters if weapon isn't fully loaded
        if (weaponAmmo != MAX_BULLET_COUNT)
		{
            //Playing sound
            AudioManager.Instance.PlaySFX("Reload");
            //Waits for reload to complete before adjusting ammo counts
            yield return new WaitForSecondsRealtime(delay);
            //When weapon is fully empty
			if (weaponAmmo == 0)
			{
                //Enters if player has enough spare ammo for full reload
                if(weaponAmmo + spareAmmo > MAX_BULLET_COUNT)
                {
					weaponAmmo = MAX_BULLET_COUNT;
					spareAmmo -= 30; 
                    EventSystem.Publish(EventSystem.eEventType.reload);
				}
                //Enters if player doesn't have enough spare ammo for full reload
                else
                {
                    weaponAmmo = spareAmmo;
                    spareAmmo = 0;
					EventSystem.Publish(EventSystem.eEventType.reload);
				}
			}
            //When weapon is not fully empty
			else
			{
                //Enters if player has enough spare ammo for full reload
                if (weaponAmmo + spareAmmo > MAX_BULLET_COUNT)
                {
                    spareAmmo -= (MAX_BULLET_COUNT - weaponAmmo);
                    weaponAmmo += (MAX_BULLET_COUNT - weaponAmmo);
                    EventSystem.Publish(EventSystem.eEventType.reload);
                }
                //Enters if player doesn't have enough spare ammo for full reload
                else
                {           
                    weaponAmmo += spareAmmo;
                    spareAmmo = 0;
					EventSystem.Publish(EventSystem.eEventType.reload);
				}
			}
            //Sets coroutine to null bc it's finished
            reloadCoroutine = null;
		}
	}

    /// <summary>
    /// For collecting ammo boxes
    /// </summary>
    void CollectPrimaryAmmo()
    {
        spareAmmo += 2 * MAX_BULLET_COUNT;
    }

    /// <summary>
    /// For stdarting ADS animation
    /// </summary>
    void startADSAnimation()
    {
        anim.Play("ADS");
    }
    
    /// <summary>
    /// For stopping ADS animation
    /// </summary>
    void stopADSAnimation()
    {
        anim.Play("Default");
    }
}
