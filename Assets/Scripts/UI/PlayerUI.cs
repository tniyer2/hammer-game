using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public static PlayerUI instance;
    public Slider strength;
    GameObject player;

    void Awake()
    {
            //Establish singleton
        if (instance == null) { instance = this; }      
        if (instance != this) { Destroy(gameObject); }
    }

	void Start ()
    {
        player = GameManager.instance.getPlayer();
	}
	
	void Update ()
    {
        if(player != null) transform.position = player.transform.position;
	}

    public void setStrength (float completed, float total)
    {
        if (total == 0 || completed >= total) strength.value = 1;
        else                                  strength.value = 1 - (total - completed) / total;
    }
}
