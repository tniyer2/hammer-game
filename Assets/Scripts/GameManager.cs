using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public static GameManager instance;     //Singleton
    public GameObject[][] inventoryItems;   //Contains entire inventory
    public GameObject[] ownedWeapons;       //Contains owned weapons in inventory
    public GameObject[] enemyList;          //List of all enemy prefabs in game
    public string[] levelNames;             //Used to hold names of all levels(except level select)
    public Vector3[] levelSpawn;            //Holds all spawn locations for each level, same order as ^^^(except level select)
    public string levelSelect;              //Name of level select scene
    public Vector3 levelSelectSpawn;        //Spawn loc for level select scene
    public GameObject menu;                 //Holds the instance of the option menu
    public StandaloneInputModule SA_IM;     //Event System's stand alone input module
    public EventSystem eSys;                //Event System
    public ButtonFunction buttonFunction;   //Holds methods for various buttons
    public GameObject playerPrefab;         //Prefab of player gameObject
    public GameObject rotatorPrefab;        //Prefab of the rotator object
    public GameObject playerUI;             //Player UI follows player around
    [HideInInspector]
    public GameObject activeWeapon;         //Prefab of active weapon player has equipped
    public float defaultMusic;              //Default sound value for music
    [HideInInspector]
    public float music;                     //Sound value for music
    public float defaultEffect;             //Default value for sound effects
    [HideInInspector]
    public float effect;                    //value for sound effects
    [HideInInspector]
    public float master;                    //Master volume

    [HideInInspector]
    public float health;                    //Health of player
    [HideInInspector]
    public bool specialInUse;               //Some special is in use
    [HideInInspector]
    public bool locked;                     //Has the player entered the platform
    [HideInInspector]
    public bool loading;                    //Is the scene loading
    [HideInInspector]
    public bool aimWithAxis;                //If the shooters should aim with axis
    [HideInInspector]
    public bool usingController;

    GameObject mainCam;                     //Stores the main camera gameObject
    GameObject player;                      //Stores the instantiated gameObject
    Vector3 offset;                         //Camera's distance from player

    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public AudioClip levelSelectMusic;
    public AudioClip level_1Music;

    public string[] controlDisplay;

    [HideInInspector]
    public string horizontalAxis, verticalAxis, aimer;
    [HideInInspector]
    public string jump, attack, special, switchWeapon, menuToggle, invToggle, turnPageRight, turnPageLeft;
    [HideInInspector]
    public string getRight, getLeft;
    public float defaultSensitivity;
    [HideInInspector]
    public float turnSensitivity;

    [HideInInspector]
    public float[] displayedValues;

    void Awake()
    {
        if (instance == null) { instance = this; }      //Establish singleton
        if (instance != this) { Destroy(gameObject); return; }
        DontDestroyOnLoad(instance);    //Doesn't destroy across multiple scenes   
        initializeVars();
    }

    void Start()
    {
        inventoryItems = new GameObject[3][];
        inventoryItems[0] = ownedWeapons;       //First row of items in inventory are weapons
        buttonFunction.changeInput(false);
        
        turnSensitivity = defaultSensitivity;
        master = 10;
        music = defaultMusic;
        effect = defaultEffect;

        getRight = turnPageRight;
        getLeft = turnPageLeft;
    }

    void OnLevelWasLoaded(int level)
    {
        initializeVars();
    }

    void Update()
    {
        mainCam.transform.position = player.transform.position + offset;                            //Move camera proportional to player
        if (Input.GetButtonDown(menuToggle) && !specialInUse) { MainMenu.instance.toggleMenu(); }   //Checks for user input if main menu should be toggled

        if (Input.GetJoystickNames().Length > 0 && !string.IsNullOrEmpty(Input.GetJoystickNames()[0]) && !usingController)
        { buttonFunction.changeInput(true); getRight = attack; getLeft = special; }
        else if (( Input.GetJoystickNames().Length == 0 || string.IsNullOrEmpty(Input.GetJoystickNames()[0]) ) && usingController)
        { buttonFunction.changeInput(false); getRight = turnPageRight; getLeft = turnPageLeft; }

        displayedValues[0] = turnSensitivity;
        displayedValues[1] = master;
        displayedValues[2] = music;
        displayedValues[3] = effect;

        audioMixer.SetFloat("MasterVol", master * 10 - 80);
        audioMixer.SetFloat("MusicVol", music * 10 - 80);
        audioMixer.SetFloat("EffectsVol", effect * 10 - 80);
    }

    void setPlayer()
    {
        if(GameObject.FindWithTag("Player") == null)
        { 
            //Instantiates player at levelSelectSpawn if scene is level select
            if (SceneManager.GetActiveScene().name == levelSelect)
            { Instantiate(playerPrefab, levelSelectSpawn, Quaternion.identity); }

            else
            {
                for (int i = 0; i < levelNames.Length; i++)      //Runs for every instance in levelNames
                {
                    //If active scene is this, instantiate player at spawn loc
                    if (SceneManager.GetActiveScene().name == levelNames[i])
                    {
                        Instantiate(playerPrefab, levelSpawn[i], Quaternion.identity);    //Instantiates player
                        Instantiate(rotatorPrefab, levelSpawn[i], Quaternion.identity);   //Instantiates rotator
                        Instantiate(playerUI, levelSpawn[i], Quaternion.identity);        //Instantiates playerUI
                    }
                }
            }
        }
    }

    void initializeVars()
    {
        setPlayer();        //instantiates the player gameObject
        player = GameObject.FindWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera");
        offset = Camera.main.transform.position - player.transform.position;
        locked = false;
        loading = false;
        displayedValues = new float[4];
        if (SceneManager.GetActiveScene().name == levelSelect)
        { audioSource.clip = levelSelectMusic; audioSource.Play(); }
        else if (SceneManager.GetActiveScene().name == levelNames[0])
        { audioSource.clip = level_1Music; audioSource.Stop(); }
    }

    public GameObject getPlayer() { return player; }

    public PlayerScript getPlayerScript() { return player.GetComponent<PlayerScript>(); }
}
