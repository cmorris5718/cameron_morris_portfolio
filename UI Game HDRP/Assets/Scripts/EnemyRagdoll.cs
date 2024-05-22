using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    float flopForce = 50f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyRagdoll(Transform playerTransform)
    {
        transform.rotation = Quaternion.LookRotation(transform.position - playerTransform.position);
        rb.AddForce(-1 * transform.forward * flopForce, ForceMode.Impulse);
    }
}
