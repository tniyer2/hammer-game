using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour {

    public GameObject weapon;
    [HideInInspector]
    public float score = 0;
    public float scoreNeeded;
    bool duplicate;

    public void activateItem()
    {
        if (GameUI.instance.secondarySlot.transform.childCount > 0)
        {
            duplicate =
            GameUI.instance.secondarySlot.transform.GetChild(0).gameObject.GetComponent<ItemUI>().weapon == weapon
            ||
            GameUI.instance.primarySlot.transform.GetChild(0).gameObject.GetComponent<ItemUI>().weapon == weapon
            ;
        }
        else if (GameUI.instance.primarySlot.transform.childCount > 0)
        {
            duplicate = GameUI.instance.primarySlot.transform.GetChild(0).gameObject.GetComponent<ItemUI>().weapon == weapon;
        }

        if (!duplicate)
        {
            GameObject temp = Instantiate(gameObject);
            GameUI.instance.onWeaponChange(temp, false);
        }
    }
}
