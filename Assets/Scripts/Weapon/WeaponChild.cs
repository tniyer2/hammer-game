using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChild : MonoBehaviour
{
    Weapon parentScript;

    void Start()
    {
        parentScript = transform.parent.GetComponent<Weapon>();
    }

    void OnCollisionEnter(Collision collision)
    {
        parentScript.OnChildCollisionEnter(collision);
    }
}
