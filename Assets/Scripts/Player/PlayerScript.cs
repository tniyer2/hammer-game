using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    public float speed, jumpSpeed,                 //How fast player can move, How fast player can turn, how fast player jumps
                       gravityMod;                 //Modifies gravity when falling back down
    public float health, fallPoint;
    [HideInInspector]
    public float defGMod;
    [HideInInspector]
    public bool immunity;                           //Player doesn't take damage in this way

    Rigidbody rb;                                   //Player object's rigidbody
    Vector3 movement;                               //Input to move player by
    bool setZandY, isGrounded = true, canJump;      //tells if can move z axis, tells if grounded on the floor
    bool repeatable;                                //If death has been invoked

	void Start () {
        defGMod = gravityMod;
        rb = gameObject.GetComponent<Rigidbody>();
            //setZ is true if on any other level than the levelSelector
        if (SceneManager.GetActiveScene().name != GameManager.instance.levelSelect)
        { setZandY = true; }
        GameManager.instance.health = health;
	}
	
	void Update ()
    {
        if ((health < 1 || transform.position.y < fallPoint) && !repeatable) //Calls death if no more health
        {
            Invoke("death", 3); repeatable = true; GameManager.instance.loading = true;
            PlayerUI.instance.gameObject.SetActive(false); gameObject.SetActive(false);
        }

        GameManager.instance.health = health;

        if (!MainMenu.instance.menuOpen)
        {
            movement.x = Input.GetAxis(GameManager.instance.horizontalAxis);
                //Only allows z movement if setZ is true
            if (setZandY) movement.z = Input.GetAxis(GameManager.instance.verticalAxis);

            if (isGrounded && Input.GetButtonDown(GameManager.instance.jump) && setZandY)      //Jump if Input
            { canJump = true; isGrounded = false; }
        }
        else movement = Vector3.zero;
            //Add gravityMod to second half of jump
        if (rb.velocity.y < 0 && !isGrounded) { rb.velocity += Vector3.up * Physics.gravity.y * gravityMod * Time.deltaTime;  }
	}

    void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);     //Moves player based on input and speed

        if (canJump)
        {
            rb.velocity += Vector3.up * jumpSpeed;
            canJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Floor")
        {
            isGrounded = true;
            rb.velocity = -rb.velocity;
        }
    }

    public void takeDamage(float damage)
    {
        if (immunity) return;
        health -= damage;
    }

    void death()
    {
        GameUI.instance.resetScore("overload");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void specialJump(float specialJumpSpeed, float newGravityMod, float time, float time2)
    {
        rb.velocity += Vector3.up * specialJumpSpeed;
        gravityMod = newGravityMod; isGrounded = false;
        Invoke("freezeY", time); Invoke("unfreezeY", time2);
    }

    void freezeY()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll; //Freezes rigidbody
    }

    public void unfreezeY()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity -= Vector3.up * rb.velocity.y;       //Freezes Y Velocity
    }
}
