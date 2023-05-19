using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    [System.Serializable]
    public struct delayInfo
    {
        public int packetSize;          //How many spawns before delays
        public float delay;             //Delay between spawns
    }

    [System.Serializable]
    public struct SpawnInfo
    {
        public int count;               //How many will be spawned
        public int enemyID;             //Index in enemy array
        public float spawnrate;         //Amount of time before each spawn
        public float initialDelay;      //Start delay
        public List<delayInfo> delays;  //Array of all delays between spawns

        [HideInInspector]
        public int p;                   //Counts how many enemies have been spawned
        [HideInInspector]
        public int d;                   //Which index in delays
        [HideInInspector]
        public int nextD;               //How many counts before next d
    }

    public SpawnInfo[] array;
    public Vector3 spawnOffset;
    public float timeMultiplier;
    bool repeatable;

    void Start()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i].spawnrate *= timeMultiplier;
            array[i].initialDelay *= timeMultiplier;
            for (int j = 0; j < array[i].delays.Count; j++)
            {
                delayInfo temp = array[i].delays[j];
                temp.delay *= timeMultiplier;
                array[i].delays[j] = temp;
            }
        }
    }

    void Update()
    {
        if (array != null && GameManager.instance.locked && !repeatable)
        {
            repeatable = true;
            for (int i = 0; i < array.Length; i++)
            {
                StartCoroutine(spawnSequence(i));
            }
        }
    }

    IEnumerator spawnSequence(int i)
    {
        SpawnInfo item = array[i];
        bool delay = false;
        item.d = 0;
        item.nextD = 0;

        if (item.initialDelay > 0) yield return new WaitForSeconds(item.initialDelay);

        for (int p = item.p; p < item.count; p++) 
        {
            if (item.delays.Count > 0 && item.delays.Count > item.d)
                delay = item.nextD == item.delays[item.d].packetSize;
            else delay = false;

            if (delay)
            {
                yield return new WaitForSeconds(item.delays[item.d].delay);
                item.nextD = 0;
                item.d++;
            }
            else yield return new WaitForSeconds(item.spawnrate);

            spawn(GameManager.instance.enemyList[item.enemyID]);
            item.nextD++;
        }
    }

    void spawn(GameObject spawnObject)
    {
        Instantiate(spawnObject, transform.position + spawnOffset, Quaternion.identity);
    }
}
