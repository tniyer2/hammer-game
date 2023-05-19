using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowScript : MonoBehaviour {

    public GameObject[] slots;                  //Holds the list of empty slot UIs.
    public GameObject rightArrow, leftArrow;    //Right and left arrows that turn the page
    public int itemRow;                         //Number of row of the array inventoryItems[][] in Gamemanager to use
    [HideInInspector]
    public bool isActive;                      //If this row of slots is active
    GameObject[] objects, garbage;              //The list of objects in each row
    int startIndex;                             //Index of objects that starts off the row

    void Start()
    {
        objects = GameManager.instance.inventoryItems[itemRow];
        garbage = new GameObject[slots.Length];
        fillRow();      //DefaultRow
    }

    void Update ()
    {
        if (isActive && objects.Length > slots.Length)      //If this row selected by player
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))       //TurnsPageRight
            {
                startIndex++;                     //Increments
                startIndex %= objects.Length;     //Wraps around if exceeds length of available slots
                fillRow();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))   //TurnsPageLeft
            {
                if (startIndex == 0) { startIndex = objects.Length - 1; }   //Don't go negative, wrap around
                else { startIndex--; }                                      //Decrease index start
                fillRow();
            }
        }
	}

    //Collects previously instantiated slots into garbage
    void slotCollect()
    {
        if (garbage != null)    //Null Exception Check
        {
            for (int i = 0; i < garbage.Length; i++)
            {
                Destroy(garbage[i]);    //Destroys all members of garbage
            }
        }
    }

    //Fills this row with sprites
    void fillRow()
    {
        slotCollect();      //Clears sprites in scene

        for (int i = 0; i < slots.Length; i++)      //Instantiates sprite object for each slot
        {
            if (i >= objects.Length) { goto here; }
            garbage[i] = fillSlot(slots[i], objects[(startIndex + i) % objects.Length]);  //Instantiates, adds to garbage for next collection
            here: ;
        }
    }

    //Fills an individual slot in this row
    GameObject fillSlot(GameObject whichSlot, GameObject whichObject)
    {
        //Instantiates Sprite
        GameObject spawnedObject = Instantiate(whichObject.transform.GetChild(0).gameObject, whichSlot.transform.position, Quaternion.identity);
        spawnedObject.transform.SetParent(whichSlot.transform);
        spawnedObject.transform.localPosition = Vector2.zero;
        return spawnedObject;       //Returns instantiated sprite
    }
}
