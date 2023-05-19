using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public float health, score;         //Enemy speed, Enemy health, Score this enemy is worth
    public float touchDamage;           //Damage dealt when touching player
    public int priority;                //Higher Priority level is more fo a threat
    protected GameObject player;        //Player gameObject
    protected GameObject audioChild;       //Child with death audio on
    protected PlayerScript ps;          //Script on player
    protected Transform playerTF;       //Player's transform
    protected Rigidbody rb;             //Enemy's rigidbody
    protected NavMeshAgent agent;       //Enemy's nav agent

    protected virtual void Start ()
    {
        player = GameManager.instance.getPlayer();
        ps = GameManager.instance.getPlayerScript();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        playerTF = player.transform;
        audioChild = transform.Find("audioChild").gameObject;
	}

    protected virtual void Update ()
    {
        if (GameManager.instance.specialInUse) agent.SetDestination(transform.position - playerTF.position);
        else                                   agent.SetDestination(playerTF.position);
            //Calls to die if health is 0
        if (health <= 0) { death(); }   
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
            //Deal damage to player if player collision
        if (collision.collider.tag == ("Player")) { ps.takeDamage(touchDamage); }
    }

    protected virtual void death()
    {
        audioChild.transform.parent = null;
        audioChild.GetComponent<AudioSource>().Play();
        Destroy(audioChild, audioChild.GetComponent<AudioSource>().clip.length);

        GameUI.instance.addScore(score);
        
        Destroy(gameObject);
    }
}
