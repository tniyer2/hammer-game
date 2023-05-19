using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour {

    public Table myTable;       //Instance of class table for this button
    public bool linksToTable;   //If clicking on this button opens another page.

    public UnityEvent bfVar;

    public void setTable()
    {
        if (linksToTable) { myTable.setAsCurrent(false); }
        bfVar.Invoke();
    }
}
