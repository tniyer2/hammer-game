using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Table {

    public GameObject[] buttons;        //Stores a button to go in each slot
    public int buffer;                  //Amount of buttons that buffer in each page
    [HideInInspector]
    public int[][] slotLocations;
    [HideInInspector]  //Page currently buffered, total amount of pages in a table, the page where the buttonof this table was clicked on.
    public int currentPage, totalPages, originPage;
    [HideInInspector]  //The table went through to get to this one
    public Table prevTable;

    void sortIndexLocations()
    {
        currentPage = 0;
        //total pages equals the amount of slots divided by how many for each page. Added to this is one if the modulus is greater than 0.
        totalPages = ((buttons.Length % buffer) > 0) ? buttons.Length / buffer + 1 : buttons.Length / buffer;
        //Initializes each array in slotLocations
        slotLocations = new int[totalPages][];
        for(int i = 0; i < totalPages; i++)
        {
            if (i + 1 == totalPages && buttons.Length % buffer != 0)
            { slotLocations[i] = new int[buttons.Length % buffer]; }    //Not all columns exist
            else
            { slotLocations[i] = new int[buffer]; }     //All columns exist
        }

        for (int i = 0; i < totalPages; i++)    //Rows(each page)
        {
            for (int j = 0; j < buffer; j++)    //Columns(each slot)
            {
                if (i * buffer + j < buttons.Length)
                {
                    slotLocations[i][j] = i * buffer + j;
                }
            }
        }
    }

    public void setAsCurrent(bool calledFromDerived)
    {
        sortIndexLocations();       //Set variables

        if (!calledFromDerived)
        {
            originPage = MainMenu.instance.currentTable.currentPage;    //Sets origin page so can go back to loc in prevTable
            prevTable = MainMenu.instance.currentTable;                 //References previous table
        }
        currentPage = MainMenu.instance.currentTable.originPage;    //Sets origin page in case this is base table
        MainMenu.instance.currentTable = this;                      //Sets this table as current table
        MainMenu.instance.slotsLoaded = false;                      //This is so variables in new table will be initialized
    }

    public void setPrevTable()
    {
        MainMenu.instance.slotsLoaded = false;                      //Sets to false so new page can be initialized
        MainMenu.instance.currentTable = prevTable;                 //Sets back to base table
        MainMenu.instance.currentTable.currentPage = originPage;    //Sets back to origin page
    }
}
