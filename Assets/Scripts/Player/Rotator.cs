using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [HideInInspector]
    public static Rotator instance;
    [HideInInspector]
    public Vector3 rotate;              //Amount to rotate rotation
    [HideInInspector]
    public Vector3 lookAtPoint;         //Point to look at
    public Vector3 offset;              //To adjust for weapon aim
    [HideInInspector]
    GameManager Manager;                //GameManager singleton
    [HideInInspector]
    GameObject player;                  //Player gameObject
    GameObject weapon;                  //Instance in scene of equipped weapon
    GameObject weaponPrefab;            //Prefab of equipped weapon
    bool weaponSet;

    void Awake()
    {
        instance = this;
    }

	void Start ()
    {
        Manager = GameManager.instance;
        player = Manager.getPlayer();
    }

    void Update()
    {
        if (weaponPrefab == null && Manager.activeWeapon == null) return;

            //Destroys instantiated weapon if activeWeapon changes, weaponSet is false for weapon not being instantiated
        if (weaponPrefab != Manager.activeWeapon) { Destroy(weapon); weaponSet = false; }

        if (!weaponSet && Manager.activeWeapon != null)
        {
            weaponPrefab = Manager.activeWeapon;
            transform.rotation = Quaternion.identity;   //Reset rotators transform
            
            weapon = Instantiate(weaponPrefab, transform.position + weaponPrefab.transform.position, weaponPrefab.transform.rotation);
                //Sets instantiated weapons parent to this, weaponSet is true for weapon being instantiated
            weaponSet = true;
            weapon.transform.SetParent(gameObject.transform);
            if (weaponPrefab.tag == "Weapon") transform.rotation *= Quaternion.AngleAxis(Random.Range(0, 360), transform.up);
        }

        //Rotates and follows player
        transform.position = player.transform.position;
        if (weapon.tag == "Weapon")
        { transform.Rotate(rotate); }
        else if (weapon.tag == "Shooter")
        {
            rotate = new Vector3(0, Input.GetAxis(GameManager.instance.aimer), 0);
            if (GameManager.instance.aimWithAxis)
            {
                transform.Rotate(rotate * GameManager.instance.turnSensitivity);
            }
            else
            {
                transform.LookAt(new Vector3(lookAtPoint.x, lookAtPoint.y, lookAtPoint.z));
                transform.Rotate(offset);
            }
        }
    }
}
