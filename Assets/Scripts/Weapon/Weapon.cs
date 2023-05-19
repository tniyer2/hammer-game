using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [HideInInspector]
    public GameManager GM;              //Reference to singleton
    public Rigidbody[] childrenRB;      //RigidBodies of all children
    public AudioClip swingClip;         //Clip that plays when gameobject is swinging
        //damage on contact, rate of rotation, rate of rotation when not activated, time until attacck, time it spins after release
    public float damage, spinrate, defSpinrate, release_t, spintime;

    protected Collider[] childrenCD;    //Colliders of weapon's children.
    protected GameObject player;
    protected Rigidbody pr;             //Player's rigidbody
    protected AudioSource audioSource;  //GameObjects audio source
    protected PlayerScript ps;          //Player's PlayerScript
        //speed to rotate y, speed to rotate z, counter for time before attack, counter for time until attack stops
    protected float yspeed, zspeed, t, t2;
        //true if x has been pressed, true if attack has begun, true if attack being implemented, true while attack
    protected bool startCount, countSpintime, inAction;

    protected virtual void Start()
    {
        player = GameManager.instance.getPlayer();
        ps = GameManager.instance.getPlayerScript();
        pr = player.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        yspeed = defSpinrate;   //Sets yspeed to spin default
            //Freezes rigidbodies of children
        childrenRB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childrenRB.Length; i++) { childrenRB[i].constraints = RigidbodyConstraints.FreezeAll; }
        childrenCD = GetComponentsInChildren<Collider>();
        disableCollider();
    }

    protected virtual void Update () {
                //if x pressed down and not in action then start attack buildup
        if (Input.GetButtonDown(GameManager.instance.attack) && !countSpintime && !inAction && !MainMenu.instance.menuOpen) { startCount = true; }
        if (startCount == true) { t += Time.deltaTime; }    //Timer till release
                //If x pushed and not in action then reset count
        if (Input.GetButtonUp(GameManager.instance.attack) && !countSpintime && !inAction)
        {
            startCount = false;
                //If buildup is enough release attack
            if (t >= release_t)
            {
                yspeed = spinrate; countSpintime = true; enableCollider();
                audioSource.clip = swingClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            t = 0;
        }

        if (countSpintime == true) { t2 += Time.deltaTime; }    //Timer till attack end
            //End attack if timer 2 has finished
        if (t2 >= spintime)
        {
            yspeed = defSpinrate; t2 = 0; countSpintime = false; disableCollider();
            audioSource.loop = false;
            audioSource.Stop();
        }

        PlayerUI.instance.setStrength(t, release_t);
            
        Rotator.instance.rotate = new Vector3(0, yspeed, zspeed);    //Rotate rotator
    }

    protected void enableCollider()
    {
        for (int i = 0; i < childrenCD.Length; i++)
        { childrenCD[i].isTrigger = false; }
    }

    protected void disableCollider()
    {
        for (int i = 0; i < childrenCD.Length; i++)
        { childrenCD[i].isTrigger = true; }
    }

    public void OnChildCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().health -= damage;
        }
    }
}
