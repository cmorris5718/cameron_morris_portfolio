using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float aggroRadius = 10f;    
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject flopEnemy;
    [SerializeField] GameObject primaryAmmoBoxPrefab;

    GameObject player;
	int health = 100;
    Rigidbody rb;
    bool chasePlayer;
    Coroutine coroutine;
    Vector3 wanderDirection;
    Animator anim;
	

	// Start is called before the first frame update
	void Start()
    {
        //This gets the overall player but not the geo 
        player = GameObject.Find("FirstPersonPlayer");
        //This switches reference to the geo associated with the player
        player = player.transform.GetChild(0).gameObject;
        Debug.Log("Found: " + player.name);
        rb = GetComponent<Rigidbody>();     
        anim = GetComponent<Animator>();
		StartCoroutine("checkDistToPlayer");
	}

    // Update is called once per frame
    void Update()
    {
		//Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.yellow);
		if (chasePlayer)
        {
            //Setting direction to the player
			Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
			direction.Normalize();
			rb.velocity = new Vector3(direction.x * moveSpeed,rb.velocity.y,direction.z * moveSpeed);
            //Setting rotation to the player
           // Debug.Log(player.transform.position - transform.position);
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;
		}
        else
        {
            rb.velocity = new Vector3(wanderDirection.x * (moveSpeed * 0.5f),rb.velocity.y,wanderDirection.z * (moveSpeed * 0.5f));
            Quaternion rotation = Quaternion.LookRotation(wanderDirection);
            transform.rotation = rotation;
        }
    }

    //Aggro range to player
    IEnumerator checkDistToPlayer()
    {
        for (; ; )
        {
            //Debug.Log("Distance to Player: " + Vector3.Distance(player.transform.position, transform.position));
            //If within aggro radius go towards player
            if (Vector3.Distance(transform.position, player.transform.position) < aggroRadius)
            {
				//Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.yellow);
				RaycastHit rayHit;
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out rayHit, aggroRadius + 1))
                {
                    //Debug.Log("Ray hit something");
                    //Debug.Log("Ray hit: " + rayHit.collider.gameObject.name);
                    if (rayHit.collider.gameObject == player)
					{
						//Debug.Log("Line of sight on player");
						chasePlayer = true;
                        //Stopping wandering coroutine
                        if(coroutine != null) StopCoroutine(coroutine);
                        coroutine = null;
                        //Setting transition for animator
                        anim.SetBool("IsAggro", true);
					}
				}       
            }
            else
            {
                chasePlayer = false;
                //Setting wandering coroutine
                if(coroutine == null)
                {
                    coroutine = StartCoroutine(pickRandomDirection());
                }
                //Setting transition for animator
                anim.SetBool("IsAggro", false);
            }
            yield return null;
        }
    }

    IEnumerator pickRandomDirection()
    {
        for (; ; )
        {
            Vector3 direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            direction.Normalize();
            wanderDirection = direction;
            //Debug.Log(direction + "Wander" + wanderDirection);
            yield return new WaitForSeconds(2f);
        }
    }

    //Handles damage from getting shot
    public void GotShot(int damage)
    {
        health -= damage;
        Debug.Log("Current Health: " + health);
        if(health <= 0)
        {
            EventSystem.Publish(EventSystem.eEventType.enemyKilled);
            Vector3 creationPos = transform.position;
            Destroy(gameObject);
            GameObject ragDoll = Instantiate(flopEnemy,transform.position,Quaternion.identity);
            //ragDoll.GetComponent<EnemyRagdoll>().ApplyRagdoll(player.transform);
            if(Random.Range(0,100) >= 50)
            {
                Instantiate(primaryAmmoBoxPrefab,transform.position, Quaternion.identity);
            }
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
        {
            //Publishing event for event system and stopping movement for attack animation
            EventSystem.Publish(EventSystem.eEventType.playerDamaged);
            StartCoroutine(StopWhileAttacking());

            //Playing attack animation
            anim.Play("Punch");
        }
	}

    IEnumerator StopWhileAttacking()
    {
        float initialTime = Time.time;
        while (initialTime + 1 > Time.time)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            yield return null;
        }
        yield return null;
    }

	//Visualization of the aggro range
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, aggroRadius);
	}
}
