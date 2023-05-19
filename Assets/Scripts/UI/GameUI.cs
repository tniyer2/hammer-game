using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    [HideInInspector]
    public static GameUI instance;                          //Singleton
    public GameObject primarySlot, secondarySlot;           //Slot for primary weapon, Slot for secondary weapon
    public Slider primarySlider, secondarySlider;           //Sliders displaying the score for each one
    public Image healthFill;                                //Fill of health slider
    public Slider health;                                   //Health Slider
    public Color fullColor, mediumColor, criticalColor;     //Colors for different percentages of health
    public float fullPer, mediumPer, criticalPer;           //Percentage of health for each stage
    [HideInInspector]
    public bool primaryUp;                                  //primary special useable? secondary special useable?
    float fullHealth;                                       //The player's initial health


    void Awake()
    {
        if (instance == null) { instance = this; }      //Establish singleton
        if (instance != this) { Destroy(gameObject); }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        fullHealth = GameManager.instance.health;       //Get original health
        healthFill.color = fullColor;                   //Set fill color to full
    }

    void Update ()
    {
        if (Input.GetButtonDown(GameManager.instance.switchWeapon) && !GameManager.instance.specialInUse) {onWeaponChange(null, true); }
        //Weapon XP stuff
        if (primarySlot.transform.childCount > 0 && primarySlot.transform.GetChild(0) != null)
        { primaryUp = primarySlot.transform.GetChild(0).GetComponent<ItemUI>().score >= primarySlot.transform.GetChild(0).GetComponent<ItemUI>().scoreNeeded; }
            //Gets percentage of health left (0-1)
        health.value =  1 - (fullHealth - GameManager.instance.health) / fullHealth;
            //Chooses health based on percntage 
        if (health.value >= fullPer) { healthFill.color = fullColor; }
        else if (health.value >= mediumPer) { healthFill.color = mediumColor; }
        else if (health.value >= criticalPer) { healthFill.color = criticalColor; }
    }

    void OnLevelWasLoaded(int level)
    {
        health.value = health.maxValue;
        primarySlider.value = 0;
        secondarySlider.value = 0;
    }

    public void onWeaponChange(GameObject temp, bool switchSlot)
    {
        GameObject temp2 = null;
        float slider1Value = primarySlider.value, slider2Value = secondarySlider.value;

        if (switchSlot)
        {
            if (secondarySlot.transform.childCount > 0)
            {
                if (primarySlot.transform.childCount > 0)
                { temp = primarySlot.transform.GetChild(0).gameObject; }
                if (secondarySlot.transform.childCount > 0)
                { temp2 = secondarySlot.transform.GetChild(0).gameObject; }

                if (temp != null)
                {
                    temp.transform.SetParent(secondarySlot.transform);
                    temp.transform.localPosition = Vector2.zero;
                    temp.transform.localScale = Vector3.one;
                }
                if (temp2 != null)
                {
                    temp2.transform.SetParent(primarySlot.transform);
                    temp2.transform.localPosition = Vector2.zero;
                    temp2.transform.localScale = Vector3.one;
                    GameManager.instance.activeWeapon = temp2.GetComponent<ItemUI>().weapon;
                }
                ItemUI p1 = primarySlot.transform.GetChild(0).GetComponent<ItemUI>();
                ItemUI p2 = secondarySlot.transform.GetChild(0).GetComponent<ItemUI>();
                primarySlider.value = 1 - (p1.scoreNeeded - p1.score) / p1.scoreNeeded;
                secondarySlider.value = 1 - (p2.scoreNeeded - p2.score) / p2.scoreNeeded;
            }
        }

        else
        {
            if (primarySlot.transform.childCount > 0)
            {
                if (secondarySlot.transform.childCount == 0)
                {
                    temp2 = primarySlot.transform.GetChild(0).gameObject;
                    temp2.transform.SetParent(secondarySlot.transform);
                    temp2.transform.localPosition = Vector2.zero;
                    temp2.transform.localScale = Vector3.one;
                    secondarySlider.value = slider1Value;
                }
                else
                {
                    Destroy(primarySlot.transform.GetChild(0).gameObject);
                }
            }
            temp.transform.SetParent(primarySlot.transform);
            temp.transform.localPosition = Vector2.zero;
            temp.transform.localScale = Vector3.one;
            GameManager.instance.activeWeapon = temp.GetComponent<ItemUI>().weapon;
            primarySlider.value = 0;
        }
    }

    public void addScore(float score)
    {
        ItemUI p;
        if (GameManager.instance.specialInUse) return;
        if (primarySlot.transform.childCount > 0)
            p = primarySlot.transform.GetChild(0).GetComponent<ItemUI>();
        else return;

        p.score += score;
        primarySlider.value = 1 - (p.scoreNeeded - p.score) / p.scoreNeeded;
    }

    public void resetScore()
    {
        if(primarySlot.transform.childCount > 0)
            primarySlot.transform.GetChild(0).GetComponent<ItemUI>().score = 0;
        primarySlider.value = 0;
    }

    public void resetScore(string overload)
    {
        if (overload == "overload")
        {
            if (primarySlot.transform.childCount > 0)
                primarySlot.transform.GetChild(0).GetComponent<ItemUI>().score = 0;
            primarySlider.value = 0;
            if (secondarySlot.transform.childCount > 0)
                secondarySlot.transform.GetChild(0).GetComponent<ItemUI>().score = 0;
            secondarySlider.value = 0;
        }
    }
}
