using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelStatus : MonoBehaviour
{

    public Button lockButton;
    public Button unlockButton;
    public Image award;

    public Animator building;

    // Update is called once per frame
    void Update()
    {
        UpdateLevelImage();
    }

    private void UpdateLevelImage()
    {

        // If user win the level, is greater than 0
        int win = PlayerPrefs.GetInt(gameObject.name);

        // Win = 2 unsuccessful
        if (win == 0 && win == 2)
        {
            Debug.Log("100");

            lockButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            award.gameObject.SetActive(false);

        }

        // Level is not locked 
        if (win == 1 && PlayerPrefs.GetInt(gameObject.name + "opened") != 1)
        {
            Debug.Log("200");
            lockButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            award.gameObject.SetActive(false);

        }

        if (win == 1 && PlayerPrefs.GetInt(gameObject.name + "opened") == 1)
        {
            Debug.Log(gameObject.name);
            award.gameObject.SetActive(true);
            lockButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
        }
    }

 

    // When you click the unlock button, unlock button dissappears and award appears
    public void ClickUnlockButton()
    {
        // Award is opened
        PlayerPrefs.SetInt(gameObject.name+"opened", 1);
        building.SetBool("building_"+gameObject.name, true);

    }
}
