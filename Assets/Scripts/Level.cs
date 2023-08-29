using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Level : MonoBehaviour
{

    public int level; // Current level of the game
    public int gridWidth;
    public LevelInfo levelInfo; // Hold level information from json data

    // Start is called before the first frame update
    void Start()
    {
        // Level is 1 at the beginning
        level = 1;
        GetJsonFileInfo();
    }

    // Update is called once per frame
    void Update()
    {   
        GetJsonFileInfo();

    }

    private void GetJsonFileInfo()
    {
        // Determine which json file will be read
        string levelString = "/Levels/level_0" + this.level + ".json";
        string json = File.ReadAllText(Application.dataPath + levelString);

        // Read level data from json file
        levelInfo = JsonUtility.FromJson<LevelInfo>(json);

        gridWidth = levelInfo.grid_width;
    }
}


[System.Serializable]
public class LevelInfo
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}