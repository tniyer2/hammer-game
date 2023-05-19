using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy {

    public Transform[] minionSpawn;
    public Transform[] projectileSpawn;
    public GameObject minion;
    public GameObject projectile;
    public float rateOfFire;
    public float tSpecial;
    float t;
    public float tSpawn;
    float t2;
    public int projectileCount;

    protected override void Update ()
    {
        base.Update();
        t += Time.deltaTime;
        t2 += Time.deltaTime;
        if (t > tSpecial) { StartCoroutine(barrage()); t = 0; }
        if (t2 > tSpawn) { spawnMinions();  t2 = 0; }
    }

    void spawnMinions()
    {
        for (int i = 0; i < minionSpawn.Length; i++)
        {
            Instantiate(minion, minionSpawn[i].position, minionSpawn[i].rotation);
        }
    }

    IEnumerator barrage()
    {
        for(int i = 0; i < projectileCount; i++)
        {
            for (int j = 0; j < projectileSpawn.Length; j++)
            {
                Instantiate(projectile, projectileSpawn[j].position, projectileSpawn[j].rotation);
            }
            yield return new WaitForSeconds(rateOfFire);
        }
    }
}
