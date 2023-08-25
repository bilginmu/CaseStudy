using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rectangle : MonoBehaviour
{
    public int width;
    public int height;
    
    public GameObject[] cellTypes; 
    public GameObject[,] allCells;
    private Level level;

    private int cellPixelHeight = 162;
    private int cellPixelWidth = 142;
    private int unitPerPixel = 100;

    // Start is called before the first frame update
    void Start()
    {    
        // There is only one Level object
        level = FindObjectOfType<Level>();

        // Get height and width from level info 
        this.height = level.levelInfo.grid_height;
        this.width = level.levelInfo.grid_width;

        allCells = new GameObject[this.height, this.width];

        CreateCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCells()
    {
        // Determine position of the cells 
        float heightUnit = (float)cellPixelHeight / unitPerPixel;
        float widthUnit = (float)cellPixelWidth / unitPerPixel;

        // Create cells according to level information
        string[] grid = level.levelInfo.grid;

        for (int i = 0; i < this.height; i++)
        {
            for (int j = 0; j < this.width; j++)
            {
                // Position are determined from left to right
                Vector2 position = new Vector2(j*widthUnit, i*heightUnit);
                
                // Determine cell type
                int cellToUse = MapFromGridStringToInteger(grid[j + this.width * i]);

                // Create cells according to level data
                GameObject cell = Instantiate(cellTypes[cellToUse], position, Quaternion.identity);
                cell.transform.parent = this.transform;
                cell.name = "( " + i + ", " + j + " )";

                // Store cell
                allCells[i, j] = cell;
            }
        }
    }



    // Map from grid string in json to tag
    int MapFromGridStringToInteger(string gridString)
    {
        switch (gridString)
        {
            case "b" : return 0;
            case "g" : return 1;
            case "r" : return 2;
            case "y" : return 3;
            case "bo" : return 4;
            case "t": return 5;
            case "s": return 6;
            case "v": return 7;
            default: return Random.Range(0,3);
        }
    }
}
