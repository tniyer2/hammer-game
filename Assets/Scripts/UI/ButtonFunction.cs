using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour {
        
    public void goHome()
    {
        SceneManager.LoadScene(GameManager.instance.levelSelect);
    }

    public void changeInput(bool controller)
    {
        if (controller)
        {
            GameManager.instance.aimWithAxis = true;
            GameManager.instance.horizontalAxis = "PS4_Horizontal";
            GameManager.instance.verticalAxis = "PS4_Vertical";
            GameManager.instance.aimer = "Aimer";
            GameManager.instance.jump = "PS4_Jump";
            GameManager.instance.attack = "PS4_Attack";
            GameManager.instance.special = "PS4_Special";
            GameManager.instance.switchWeapon = "PS4_Switch";
            GameManager.instance.menuToggle = "PS4_Escape";
            GameManager.instance.invToggle = "PS4_InvToggle";
            GameManager.instance.SA_IM.horizontalAxis = "PS4_Horizontal";
            GameManager.instance.SA_IM.verticalAxis = "PS4_Vertical";
            GameManager.instance.SA_IM.submitButton = "PS4_Jump";
            GameManager.instance.usingController = true;
        }
        else
        {
            GameManager.instance.aimWithAxis = false;
            GameManager.instance.horizontalAxis = "Horizontal";
            GameManager.instance.verticalAxis = "Vertical";
            GameManager.instance.jump = "Jump";
            GameManager.instance.attack = "Attack";
            GameManager.instance.special = "Special";
            GameManager.instance.switchWeapon = "Switch";
            GameManager.instance.menuToggle = "Escape";
            GameManager.instance.invToggle = "InvToggle";
            GameManager.instance.SA_IM.horizontalAxis = "Horizontal";
            GameManager.instance.SA_IM.verticalAxis = "Vertical";
            GameManager.instance.SA_IM.submitButton = "Submit";
            GameManager.instance.usingController = false;
        }
    }

    public void changeSensitivity(float amount)
    {
        if(!(GameManager.instance.turnSensitivity + amount > 10 || GameManager.instance.turnSensitivity + amount < 1))
            GameManager.instance.turnSensitivity += amount;
    }

    public void defaultSensitivity()
    {
        GameManager.instance.turnSensitivity = GameManager.instance.defaultSensitivity;
    }

    public void defaultMusic()
    {
        GameManager.instance.music = GameManager.instance.defaultMusic;
    }

    public void changeMusic(float amount)
    {
        if (!(GameManager.instance.music + amount > 10 || GameManager.instance.music + amount < 0))
            GameManager.instance.music += amount;
    }

    public void defaultEffect()
    {
        GameManager.instance.effect = GameManager.instance.defaultEffect;
    }

    public void changeEffect(float amount)
    {
        if (!(GameManager.instance.effect + amount > 10 || GameManager.instance.effect + amount < 0))
            GameManager.instance.effect += amount;
    }

    public void defaultMaster()
    {
        GameManager.instance.master = 10;
    }

    public void changeMaster(float amount)
    {
        if (!(GameManager.instance.master + amount > 10 || GameManager.instance.master + amount < 0))
            GameManager.instance.master += amount;
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
