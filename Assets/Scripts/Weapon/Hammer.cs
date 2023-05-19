using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon{

    public AudioClip spinClip, smashClip;
    public GameObject mesh, blastR;         //Child object reference, <--, reference to blast radius object 
    public Material gold, grid, black, grey;    //Materials for the object
    public Light glow;                          //Light attached to weapon object, activates while special
    public Vector3 camRotation;
    public float jumpSpeed, jumpTime, fallTime, gravityMod,
                 intervalX, intervalY, intervalZ, slamPoint, 
                 rtnPoint, specialSpeed, spins, specialPoint, blastT, camFOW;
    
    [HideInInspector]
    public delegate void del(float a, float b);         //Holds a specific version of a function that spins the weapon
    GameObject blastInst; 
    bool[] arraySet;                            //Holds the values for each function set. If set called after first call.
    float j, j2, distance, speed;
    bool specialActive;

    protected override void Start()
    {
        arraySet = new bool[3];
        base.Start();
    }

    protected override void Update()
    {
            //Timer 1, counts special
        if (specialActive) j += Time.deltaTime;
            //Input
        if (Input.GetButtonDown(GameManager.instance.special) && !startCount && !countSpintime && GameUI.instance.primaryUp && !MainMenu.instance.menuOpen)
        {
            yspeed = 0; specialActive = true; inAction = true; GameUI.instance.resetScore(); GameManager.instance.specialInUse = true;
            audioSource.clip = spinClip; audioSource.Play();
        }
            //Stop Special. This is for setting every variable
        if (j > intervalZ) resetAll();
            //HammerLift
        else if (j > intervalY) spinMode(intervalY, intervalZ, rtnPoint, 0, 1, ref arraySet[2], _return);
            //HammerSlam
        else if (j > intervalX) spinMode(intervalX, intervalY, slamPoint, 0, -1, ref arraySet[1], slam);
            //HammerSpin
        else if (j > 0) spinMode(0, intervalX, 0, spins, -1, ref arraySet[0], spin);

        if (blastInst != null) { j2 += Time.deltaTime;
            if (j2 > blastT) { Destroy(blastInst); j2 = 0; } }

        base.Update();
    }

    protected virtual void spinMode(float timeA, float timeB, float destination, float spin, int dir, ref bool set, del dOnce)
    {
            //if spinMode has been called for the first time then set distance
            //Distance set to rotation of rotator away from destination rotation, plus the amount of extra spins wanted
        if (!set) { dOnce(timeA, timeB); distance = Mathf.Abs(Rotator.instance.transform.rotation.eulerAngles.z - destination) + (360 * spin); set = true; }
            //Speed is distance over (final time minus start time) multiplied by deltaTime
        speed = Time.deltaTime * (distance / (timeB - timeA));
            //Rotates amount to be rotated
        Rotator.instance.transform.rotation *= Quaternion.AngleAxis((dir * speed), Vector3.forward);
    }

    void resetAll()
    {
        yspeed = defSpinrate; j = 0; specialActive = false; arraySet = new bool[3]; inAction = false;
        glow.enabled = false;
        mesh.transform.rotation *= Quaternion.AngleAxis(90, Vector3.forward);
        Rotator.instance.transform.rotation = Quaternion.Euler(0, Rotator.instance.transform.rotation.y, 0);
        GameManager.instance.specialInUse = false; ps.immunity = false;
        audioSource.Stop();
    }

    void spin(float a, float b)
    {
            //Sets hammer to certain beginning location
        Rotator.instance.transform.eulerAngles = new Vector3(Rotator.instance.transform.eulerAngles.x, specialPoint, 0);
            //Rotates the hammer head so that short sides are facing down
        mesh.transform.rotation *= Quaternion.AngleAxis(-90, Vector3.forward);
            //Starts coroutine zoomInOut
        StartCoroutine(CamZoomer.instance.zoomInOut( camRotation, camFOW, 0.2f, intervalZ - intervalY, intervalX) );
            //Special effects
        glow.enabled = true;
            //Makes player immune and jumps
        ps.immunity = true; ps.specialJump(jumpSpeed, gravityMod, jumpTime, fallTime);
    }

    void slam(float a, float b)
    {
            //Makes sure z rotation stays 0
        Rotator.instance.transform.eulerAngles = new Vector3(Rotator.instance.transform.eulerAngles.x, Rotator.instance.transform.eulerAngles.y, 0);
    }

    void _return(float a, float b)
    {
        blastInst = Instantiate(blastR, player.transform.position, Quaternion.identity);
        ps.gravityMod = ps.defGMod;         //Return player to default gravity
        audioSource.Stop(); audioSource.clip = smashClip; audioSource.Play();
    }
}
