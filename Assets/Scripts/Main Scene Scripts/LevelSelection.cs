using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public Button lockButton;
    public Button unlockButton;
    public Image award;

    public Animator building;

    private int levelIndex;

    public TMP_Text levelButtonText;

    void Start()
    {
        levelIndex = 1;
    }

    private void Update()
    {
        UpdateLevel();
        UpdateLevelButtonText();
    }


    private void UpdateLevel()
    {
        // Level 1 is successfully finished
        if (PlayerPrefs.GetInt("level_01") == 1)
        {
            levelIndex = 2;
        }
        // Level 2 is successfully finished
        if (PlayerPrefs.GetInt("level_02") == 1)
        {
            levelIndex = 3;
        }

        // Level 3 is successfully finished
        if (PlayerPrefs.GetInt("level_03") == 1)
        {
            levelIndex = 4;
        }
        // Level 4 is successfully finished
        if (PlayerPrefs.GetInt("level_04") == 1)
        {
            levelIndex = 5;
        }

        // Level 5 is successfully finished
        if (PlayerPrefs.GetInt("level_05") == 1)
        {
            levelIndex = 6;
        }
        // Level 6 is successfully finished
        if (PlayerPrefs.GetInt("level_06") == 1)
        {
            levelIndex = 7;
        }

        // Level 7 is successfully finished
        if (PlayerPrefs.GetInt("level_07") == 1)
        {
            levelIndex = 8;
        }

        // Level 8 is successfully finished
        if (PlayerPrefs.GetInt("level_08") == 1)
        {
            levelIndex = 9;
        }

        // Level 9 is successfully finished
        if (PlayerPrefs.GetInt("level_09") == 1)
        {
            levelIndex = 10;
        }

        // Level 10 is successfully finished
        if (PlayerPrefs.GetInt("level_10") == 1)
        {
            levelIndex = 11;
        }
    }


    private void UpdateLevelButtonText()
    {
        if (levelIndex == 11)
        {
            levelButtonText.text = "Finished";

        }
        else
        {
            levelButtonText.text = "Level " + levelIndex.ToString();
        }
    }



    // Click level button to load new level
    public void ClickLevelButton()
    {
        if (levelIndex == 11)
        {
            return;
        }

        string levelStr;
        if (levelIndex != 10)
        {
            levelStr = "level_0" + levelIndex.ToString(); 
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            levelStr = "level_" + levelIndex.ToString();
        }
       
        SceneManager.LoadScene(levelStr);
    }


}
