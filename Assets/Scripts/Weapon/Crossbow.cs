using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Shooter {

    [System.Serializable]
    public struct PathSequence                  //Holds a sequence of paths
    {
        public bool symmetry;                               //Should you spawn the opposite path sequence? 
        public List <Projectile.Path> paths;                //List of paths
        [HideInInspector]
        public List<Projectile.Path> symmetricalPaths;      //List of symmetrical paths
    }

    public AudioClip specialClip;               //AudioClip that plays while special
    public PathSequence[] array_pathSeq;        //Array of pathsequences
    public GameObject specialProjectile;        //Special Arrow that 
    public float specialRateOfFire;             //Rate of fire when special firing
    public float specialLength;                 //How long will the special last
    float specialTimer;                         //Timer counts till end of special, Field view of main camera before special
    bool specialActive;                         //Is the special active

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < array_pathSeq.Length; i++)      //--------------------------------------------------------
        {                                                   //For every PathSequence in array that has symmetry = true
            if (array_pathSeq[i].symmetry)                  //--------------------------------------------------------
            {
                //Symmetrical Path is equal to the regular path
                array_pathSeq[i].symmetricalPaths = new List<Projectile.Path>(array_pathSeq[i].paths);
                for (int j = 0; j < array_pathSeq[i].paths.Count; j++)  //For every path in PathSequence
                {
                    if (!array_pathSeq[i].paths[j].keepDirection)
                    {
                        Projectile.Path path = array_pathSeq[i].symmetricalPaths[j];    //temp = path
                        path.direction *= -1;       //Invert the direction
                        array_pathSeq[i].symmetricalPaths[j] = path;                    //path = temp
                    }
                }
            }
        }
    }

    protected override void Update()
    {
            //Once after Input to fire
        if (Input.GetButtonDown(GameManager.instance.special) && GameUI.instance.primaryUp && !MainMenu.instance.menuOpen && !specialActive)
        { specialActive = true; fireNextShot = false; StartCoroutine(specialShoot()); }
            //Tick Timer
        if (specialActive) { specialTimer += Time.deltaTime; }
        base.Update();
    }

    IEnumerator specialShoot()
    {
            //Play Audio
        audioSource.clip = specialClip; audioSource.Play(); audioSource.loop = true;
            //Set variables
        GameManager.instance.specialInUse = true;
        GameUI.instance.resetScore();
            //As long as special is not over
        while(specialTimer < specialLength)
        {
            for (int i = 0; i < array_pathSeq.Length; i++)
            {
                instantiateProjectile(projectile, array_pathSeq[i].paths);
                if (array_pathSeq[i].symmetry)      //Instantiate with symmetrical paths if it's not null
                {
                    instantiateProjectile(projectile, array_pathSeq[i].symmetricalPaths);
                }
            }
            yield return new WaitForSeconds(specialRateOfFire);     //Wait until next shot available
        }
        instantiateProjectile(specialProjectile, array_pathSeq[0].paths);
            //Reset all variable, spcial is over
        specialActive = false; specialTimer = 0; fireNextShot = true; GameManager.instance.specialInUse = false;
            //Stop Audio
        audioSource.loop = false; audioSource.Stop(); 
    }

    void instantiateProjectile(GameObject prefab, List<Projectile.Path> path)
    {
        GameObject temp = Instantiate(prefab, projectileSpawn.transform.position, prefab.transform.rotation);
            //Rotates projecile by the amount rotator is rotated
        temp.transform.Rotate(Rotator.instance.transform.rotation.eulerAngles);
            //Set Projectiles list of paths equal to this list of paths
        temp.GetComponent<Projectile>().aimPaths = new List<Projectile.Path>(path);
    }
}
