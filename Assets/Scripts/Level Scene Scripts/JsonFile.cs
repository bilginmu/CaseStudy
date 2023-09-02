using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class JsonFile : MonoBehaviour
{

    private LevelInformation levelInformation;
    private void Start()
    {
        levelInformation = new LevelInformation();
    }


    public void GetJsonFileInfo(string levelStr)
    {
        // Determine which json file will be read
        string levelString = "/Levels/" + levelStr + ".json";
        string json = File.ReadAllText(Application.dataPath + levelString);

        // Read level data from json file
        levelInformation = JsonUtility.FromJson<LevelInformation>(json);

    }

    public int GetHeight()
    {
        return levelInformation.grid_height;
    }

    public int GetWidth()
    {
        return levelInformation.grid_width;
    }

    public int GetMoveCount()
    {
        return levelInformation.move_count;
    }

    public string[] GetGrid()
    {
        return levelInformation.grid;
    } 


}




[System.Serializable]
public class LevelInformation
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}