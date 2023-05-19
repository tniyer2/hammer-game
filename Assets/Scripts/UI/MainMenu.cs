using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [HideInInspector]
    public static MainMenu instance;                //Singleton
    public GameObject[] headers, rows;              //Holds the header for each screen view
    public Transform[] buttonStartPositions;        //Transform holding the anchor positions of the button start positions
    public GameObject returnButton, inventoryPage;  //Button to go to previous table, page where inventory is on
    public Table mainTable;                         //Main Table which contains slots
    [HideInInspector]
    public Text[] displayedValues;                  //Contains list of texts from buttons which show values  
    [HideInInspector]
    public Table currentTable;                      //Table being currently buffered
    [HideInInspector]
    public bool slotsLoaded;                        //If there is text on the screen
    [HideInInspector]
    public bool menuOpen, inInventory;
    public float delay;
    GameObject[] buttonGarbage;                     //References to buttons in scene so they can be garbage collected
    int activeRow, activeSlot, activeButton;        //Index of row, slot, and button that is active
    bool turnPageRight, turnPageLeft;
    bool[] setInput = { true, true};

    void Awake()
    {
        if(instance == null) { instance = this; }        //Initialize Singleton
        if (instance != this) { Destroy(gameObject); }
        DontDestroyOnLoad(instance);  //Keeps for all scenes
        gameObject.SetActive(false);    //Hides main menu on load after singleton has been initialized
    }

	void Start ()
    {
        inInventory = true;
        currentTable = mainTable;           //Main table should be current table at start
        currentTable.setAsCurrent(true);
        buttonGarbage = new GameObject[0];
        rows[0].GetComponent<RowScript>().isActive = true;
        GameManager.instance.eSys.SetSelectedGameObject(rows[activeRow].transform.GetChild(0).GetChild(0).gameObject);
    }

    void Update()
    {
        if (setInput[0])
        {
            if (Input.GetButtonDown(GameManager.instance.getRight)  && !inInventory)             //Turn left if left arrow
            { turnPageRight = true; setInput[0] = false; StartCoroutine(resetBool(0, delay)); }
            else if (Input.GetButtonDown(GameManager.instance.getLeft) && !inInventory)        //Turn right if right arrow
            { turnPageLeft = true; setInput[0] = false; StartCoroutine(resetBool(0, delay)); }
        }

        if (setInput[1])
        {
            if (Input.GetAxis(GameManager.instance.verticalAxis) > 0 && inInventory)            //Select Row Above this one
            { selectRow(true); setInput[1] = false; StartCoroutine(resetBool(1, delay));} 
            else if (Input.GetAxis(GameManager.instance.verticalAxis) < 0 && inInventory)       //Select Row Below this one
            { selectRow(false); setInput[1] = false; StartCoroutine(resetBool(1, delay)); }
        }

        if (Input.GetButtonDown(GameManager.instance.invToggle))   //toggles between inventory and options
        {
            inInventory = !inInventory;
            if (!inInventory) { gotoOptions(true); }
            else { gotoOptions(false); }
        }

        if (menuOpen)   //If The Menu has been Opened
        {
            if (((turnPageLeft || turnPageRight) || !slotsLoaded) && !inInventory)   //If signal to turn page or no slots loaded, and the Menu screen is active
            {
                GameManager.instance.eSys.SetSelectedGameObject(returnButton);
                instantiatePage(); inventoryPage.SetActive(false);
            }
        }
	}

    void OnEnable()
    {
        menuOpen = true;
    }

    void OnDisable()
    {
        menuOpen = false;
        slotsLoaded = false;
        currentTable.currentPage = 0;   //Reset the memory of page location
        currentTable = mainTable;       //Make the current table the main table
        buttonCollect();                //Garbage Collection for buttons in scene
        activeRow = 0; activeSlot = 0; activeButton = -1;
        if(inInventory) GameManager.instance.eSys.SetSelectedGameObject(rows[activeRow].transform.GetChild(0).GetChild(0).gameObject);
    }

    void gotoOptions(bool go)
    {
        if (go == true)
        {
            slotsLoaded = false; returnButton.SetActive(true);
            activeRow = 0; activeSlot = 0; activeButton = -1;
        }
        else
        {
            buttonCollect(); returnButton.SetActive(false);
            inventoryPage.SetActive(true);
            GameManager.instance.eSys.SetSelectedGameObject(rows[activeRow].transform.GetChild(0).GetChild(0).gameObject);
        }
    }

    //Destroys each button set for garbage collection
    void buttonCollect()
    {
        if (buttonGarbage != null)
        {
            for (int i = 0; i < buttonGarbage.Length; i++)
            { Destroy(buttonGarbage[i]); }
        } 
    }

    //Activated row
    void selectRow(bool up)
    {
        if (up && activeRow != 0)     //Rows are farther back in array if located up
        {
            rows[activeRow].GetComponent<RowScript>().isActive = false;     //Disactivates former row
            activeRow--;
        }
        else if(!up && activeRow < rows.Length - 1)    //Rows are farther up in array if located down
        {
            rows[activeRow].GetComponent<RowScript>().isActive = false;     //Disactivates former row
            activeRow++;
        }
        rows[activeRow].GetComponent<RowScript>().isActive = true;      //Activates Row
    }

    //Instantiates a new button
    void instantiateButton(GameObject buttonObject, Vector3 spawnPoint, int index)
    { 
        GameObject temp;                        //Holds reference to instantiated obj
        temp = Instantiate(buttonObject);       //Instantiates
        buttonGarbage[index] = temp;            //Adds newly instantiated button into garbage collection
        temp.transform.SetParent(transform);     //Sets button transform to child of canvas this script is attached to
        temp.GetComponent<RectTransform>().position = spawnPoint;
    }

    //Creates an entire page
    void instantiatePage()
    {
        if (turnPageLeft && (currentTable.currentPage - 1 >= 0)) { currentTable.currentPage--; }    //Decrease currentPage if can
        if (turnPageRight && (currentTable.currentPage + 1 < currentTable.totalPages)) { currentTable.currentPage++; }  //Increase currentPage if can

        buttonCollect();    //Remove previous page

        buttonGarbage = new GameObject[currentTable.buffer];    //Reset garbage collection array to new buffer size

        for (int j = 0; j < currentTable.buffer; j++)   //Runs for buffer size of one page
        {
            //Skips for loop if on the index on the last page that is out of bounds
            if (currentTable.currentPage == currentTable.totalPages - 1)
            {
                if (currentTable.slotLocations[currentTable.totalPages - 1].Length == j) { goto here; }
            }

            if (currentTable.buttons[currentTable.currentPage * currentTable.buffer + j] != null)   //Null exception check
            {
                instantiateButton(currentTable.buttons[currentTable.currentPage * currentTable.buffer + j], buttonStartPositions[j].position, j);
            }
        }
        here:;  //For Loop ^^^ exit
                //Resets all variables required for one load
        turnPageLeft = false;
        turnPageRight = false;
        slotsLoaded = true;
    }

    //Switches active menu to not and vice versa
    public void toggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    //Opens up the previous table
    public void goBack()
    {
        if (currentTable != mainTable)
        {
            currentTable.prevTable.setAsCurrent(true);
        }
    }

    IEnumerator resetBool(int index, float time)
    {
        yield return new WaitForSeconds(time);
        setInput[index] = true;
    }
}
