using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDisplay : MonoBehaviour {

    public Text text;
    public Vector3 optionsPosition, inventoryPosition;
    public int index;
    public bool showInInventory, showInOptions;
    GameObject child;

	void Start ()
    {
        child = transform.GetChild(0).gameObject;
	}
	
	void Update ()
    {
        if (MainMenu.instance.inInventory)
        {
            if (child.activeInHierarchy && !showInInventory) child.SetActive(false);
            if (!child.activeInHierarchy && showInInventory) child.SetActive(true);
            child.transform.localPosition = inventoryPosition;
        }
        else
        {
            if (child.activeInHierarchy && !showInOptions) child.SetActive(false);
            if (!child.activeInHierarchy && showInOptions) child.SetActive(true);
            child.transform.localPosition = optionsPosition;
        }

        if (GameManager.instance.usingController) text.text = GameManager.instance.controlDisplay[index + 1];
        else                                      text.text = GameManager.instance.controlDisplay[index];
	}
}
