using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject projectileSpawn;  //Spawn area of projectile
    public GameObject projectile;       //The prefab of the projectile
    public float rateOfFire;            //Time(Seconds) before each new shot is available
    public AudioClip fireClip;          //Made when firing projectile
    protected GameObject player;
    protected Rigidbody[] childrenRB;             //Array of children's Rigidbodies
    protected AudioSource audioSource;            //GameObject's audio source
    protected Plane p;                            //Plane the player is on
    protected Ray mouseRay;                       //Ray coming out of cursor and onto world space
    protected float distanceOnRay;                //Distance along Ray ray where it intersects plane p
    protected float t;                            //Times until next shot
    protected bool fireNextShot;                  //If next shot is available to fire

	protected virtual void Start ()
    {
        player = GameManager.instance.getPlayer();
        fireNextShot = true;
        childrenRB = GetComponentsInChildren<Rigidbody>();      // <---- && VVV Freezes all children
        for (int i = 0; i < childrenRB.Length; i++) { childrenRB[i].constraints = RigidbodyConstraints.FreezeAll; }
        audioSource = GetComponent<AudioSource>();
    }
	
	protected virtual void Update ()
    {
        p = new Plane(-Vector3.up, player.transform.position.y);         //Represents players y loc
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);   //cursor(screenpoint) converted to ray in worldspace
        
        if (p.Raycast(mouseRay, out distanceOnRay))
        {
            Rotator.instance.lookAtPoint = mouseRay.GetPoint(distanceOnRay);
        }
        
        if (Input.GetButtonDown(GameManager.instance.attack) && fireNextShot && !MainMenu.instance.menuOpen)  //If next shot available and x down
        {
            instantiateProjectile();                            //Fire
            audioSource.clip = fireClip; audioSource.Play();    //Play firing sound
            fireNextShot = false;                               //You can't fire again until VVV
            t = 0;                                              //Reset Timer
            Invoke("timer", rateOfFire);                        //Reset ability to fire after this much time(rateOfFire)
        }

        t += Time.deltaTime;
        PlayerUI.instance.setStrength(t, rateOfFire);
	}

    //Instantiates Projectile
    protected virtual void instantiateProjectile()
    {
        GameObject temp = Instantiate(projectile, projectileSpawn.transform.position, projectile.transform.rotation);
            //Rotates projecile by the amount rotator is rotated
        temp.transform.Rotate(Rotator.instance.transform.rotation.eulerAngles);         
    }

    //Used as a function to pass through invoke, acting like a timer
    protected void timer()
    {
        fireNextShot = true;
    }
}
