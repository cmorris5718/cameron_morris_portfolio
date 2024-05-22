using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float aggroRadius = 10f;

    Rigidbody rb;
    Vector3 wanderDirection;
    int health = 100;
    bool chasePlayer = false;
    Coroutine coroutine;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coroutine = StartCoroutine(pickRandomDirection());
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(wanderDirection);

        transform.rotation = rotation;
        rb.velocity = wanderDirection.normalized * moveSpeed;
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
}
