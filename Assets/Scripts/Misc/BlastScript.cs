using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastScript : MonoBehaviour {

    public float damage;
    public float rate;
    float timer;

    void OnTriggerEnter(Collider other)
    {
        timer = Time.deltaTime;
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().health -= damage;
        }
    }

    void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        if (other.tag == "Enemy")
        {   if (timer >= rate)
            {
                other.gameObject.GetComponent<Enemy>().health -= damage;
                timer = 0;
            }
        }
       
    }
}
