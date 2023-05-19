using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

    public string id;           //Id of the level the door teleports to
    float doorSize;     //Length on x axis of parent object
    float playerX;      //x position of player
    float doorX;        //x position of door
    GameObject player;          //player's gameobject

    void Start()
    {
        player = GameManager.instance.getPlayer(); ;
        doorSize = transform.localScale.x;
    }

    void Update ()
    {
        playerX = player.transform.position.x;  doorX = transform.position.x;
            //If player is inside range of door's x position and input then load level
		if(playerX >= doorX - doorSize && playerX <= doorX + doorSize && Input.GetButtonDown(GameManager.instance.jump) && !MainMenu.instance.menuOpen) SceneManager.LoadScene(id);
	}
}
