using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] AmmoType.ammoType type;

    public AmmoType.ammoType ammoType
    {
        get { return type; }
    }
}
