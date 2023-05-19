using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour {

    public GameObject[] barriers;
    public GameObject[] locks;
    Rigidbody[] lockRB;
    AudioSource audioSource;
    float timer;
    bool repeatable1, repeatable2;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (repeatable1)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                for (int i = 0; i < locks.Length; i++)
                {
                    if (lockRB[i].velocity == Vector3.zero) onKinematic(lockRB[i]);
                }
            }
        }

        if (!repeatable2 && !audioSource.isPlaying && GameManager.instance.locked)
        {
            GameManager.instance.audioSource.Play();
            repeatable2 = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!repeatable1 && other.tag == "Player")
        {
            repeatable1 = true;
            GameManager.instance.locked = true;
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i].SetActive(true);
            }
            lockRB = new Rigidbody[locks.Length];
            for (int i = 0; i < locks.Length; i++)
            {
                locks[i].SetActive(true);
                lockRB[i] = locks[i].GetComponent<Rigidbody>();
            }
            audioSource.Play();
        }
    }

    void onKinematic(Rigidbody r)
    {
        r.isKinematic = true;
    }
}
