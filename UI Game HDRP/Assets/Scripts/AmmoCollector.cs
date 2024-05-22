using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Checking ammo type & calling appropriate method
        if (other.GetComponent<AmmoBox>().ammoType == AmmoType.ammoType.PrimaryAmmo)
        {
            CollectPrimaryAmmo();
        }
        else
        {
            CollectSecondaryAmmo();
        }
        Destroy(other.gameObject);
    }

    void CollectPrimaryAmmo()
    {
        EventSystem.Publish(EventSystem.eEventType.collectPrimaryAmmo);
        EventSystem.Publish(EventSystem.eEventType.reload);
    }

    void CollectSecondaryAmmo()
    {

    }
}
