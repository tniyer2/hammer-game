using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Projectile : MonoBehaviour {

    [System.Serializable]
    public struct Path              //Characteristics of each path aimer goes.
    {
        public bool keepDirection;
        public Vector3 direction;   //The local point at which the transform is aiming at
        public float x_degrees;     //How much should it rotate on the x axis
        public float y_degrees;     //How much should it rotate on the y axis
        public float timeLength;    //Time it path will take
        [HideInInspector]
        public float timeCounted;   //How much time has been completed
    }
    public List<Path> aimPaths;     //List of all paths that should be taken
    public GameObject package;                      //Homing projectilem bomb, etc...
    public string targetTag;                        //Tag of the object that should take damage
    public float speed,                             //Speed of projectile
        minDamage, maxDamage,                       //Damage starts out with, damage after covering optimum
        optimumDistance;                            //Distance giving maxDamage whenn reached, distance from starting location
    public bool delivers, isHomer;                  //If this arrow delivers special, is this projectile a package
    Rigidbody rb;                                   //Projectile's Rigidbody
    Vector3 rotation;                               //How much this object should rotate
    Vector3 startPosition;                          //Spawn Position of Projectile
    float damage, distanceTravelled;                //Current damage value
    int i = 0;                                      //Used for iterating through aimPaths

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        damage = minDamage;
    }

    void Update()
    {
        appreciateDamage();
        getMovement();
        transform.Rotate(rotation);
        if(isHomer && targetTag == "Player")
        { transform.LookAt(GameManager.instance.getPlayer().transform.position); }
        else if (isHomer && GameObject.FindWithTag(targetTag) != null)
        { transform.LookAt(findTarget()); }
            //Add force before rotating
        rb.velocity = transform.forward * speed;
    }

    void appreciateDamage()
    {
        if (distanceTravelled == optimumDistance || optimumDistance == 0) damage = maxDamage;
        else
        {
            distanceTravelled = Mathf.Abs(Vector3.Distance(startPosition, transform.position));
            float interpolation = 1 - (optimumDistance - distanceTravelled) / optimumDistance;
            damage = Mathf.Lerp(minDamage, maxDamage, interpolation);
        }
    }

    //Calculated amount transform should be moved
    void getMovement()
    {
        rotation = Vector3.zero;
        Path temp = aimPaths[i];

        temp.timeCounted += Time.deltaTime;     //Updates time that has passed
        if (temp.timeCounted == Time.deltaTime)
        { rotation = linearPath(temp.direction, temp.x_degrees, temp.y_degrees); }    //Only have to do once

        aimPaths[i] = temp;
        if (aimPaths[i].timeCounted >= aimPaths[i].timeLength)
        {
            if (i + 1 == aimPaths.Count) { Destroy(gameObject); }   //Destroys projectile if aimPaths is empty
            aimPaths.RemoveAt(i);   //Destroys object if lifespan has been completed
        }
    }

    //Finds how much to move transform given the amount of time in a linear path to destination
    Vector3 linearPath(Vector3 dir, float x_amount, float y_amount)
    {
        return new Vector3 (dir.x * x_amount, dir.y * y_amount, dir.z);
    }

    Vector3 findTarget()
    {
        var closestGameObject = GameObject.FindGameObjectsWithTag(targetTag)
        .Select(go => go.transform)
        .OrderByDescending(t => t.GetComponent<Enemy>().priority)
        .ThenBy(t => Vector3.Distance(t.transform.position, transform.position))
        .FirstOrDefault();

        if(closestGameObject != null)
            return closestGameObject.transform.position;
        return Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (targetTag == "Enemy")
            { other.gameObject.GetComponent<Enemy>().health -= damage; }
            else if (targetTag == "Player")
            { other.gameObject.GetComponent<PlayerScript>().takeDamage(damage); }
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (delivers && !GameManager.instance.loading)
        {
            Vector3 pos = transform.position;
            transform.LookAt( new Vector3(pos.x, Mathf.Abs(pos.y) - pos.y, pos.z) );        //Looks down
            GameObject temp = Instantiate(package, transform.position, transform.rotation); //Spawns projectile
            temp.SetActive(true);
        }
    }

}
