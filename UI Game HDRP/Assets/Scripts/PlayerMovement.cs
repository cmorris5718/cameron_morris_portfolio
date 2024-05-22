using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Camera playerCamera;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider armorSlider;

    Vector3 velocity;
    public bool isGrounded;
    bool isActive = true;
    float switchTime = 0f;
    int health = 100;
    int armor = 100;

    // Start is called before the first frame update
    void Start()
    {
		EventSystem.Subscribe(EventSystem.eEventType.switchInput, switchInput);
        EventSystem.Subscribe(EventSystem.eEventType.playerDamaged, takeDamage);
        hpSlider.maxValue = 100;
        hpSlider.minValue = 0;
        hpSlider.value = health;
        armorSlider.value = armor;
	}

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            //Debug.Log("Player health" + health);
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //Setting left/right straffing to be less fast than forward backward movement
            x *= 0.8f;

            Vector3 move = transform.right * x + transform.forward * z;
            characterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            //Switching to player
            if(switchTime == Time.time)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
				Debug.Log("Switching Character");
                EventSystem.Publish(EventSystem.eEventType.switchInput);
                playerCamera.gameObject.SetActive(false);
			}
		}
    }

	void switchInput()
	{
        Debug.Log("PlayerSwitchInput");
		isActive = !isActive;
        switchTime = Time.time;
        if(isActive)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }
        AudioManager.Instance.StopGunSFX();
    }

    void takeDamage()
    {
        health -= 10;
        Vector3 knockback = -1 * transform.forward;
        float knockbackForce = 250f;
        characterController.Move(knockback * knockbackForce * Time.deltaTime);
        if(health <= 0)
        {
            EventSystem.Publish(EventSystem.eEventType.playerDied);
            Cursor.lockState = CursorLockMode.Confined;
            Destroy(GameObject.Find("AudioManager"));
        }
        hpSlider.value = health;
    }
}
