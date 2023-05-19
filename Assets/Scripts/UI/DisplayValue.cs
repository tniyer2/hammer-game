using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayValue : MonoBehaviour {

    public string textString;
    public int index;
    Text text;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Update ()
    {
        text.text = textString + " " + GameManager.instance.displayedValues[index];
	}
}
