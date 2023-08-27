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

    public int cellPixelHeight = 162;
    public int cellPixelWidth = 142;
    public int unitPerPixel = 100;

    // Start is called before the first frame update
    void Start()
    {    
        // There is only one Level object
        level = FindObjectOfType<Level>();

        // Get height and width from level info 
        this.height = level.levelInfo.grid_height;
        this.width = level.levelInfo.grid_width;

        // Top row for creating new cells
        allCells = new GameObject[this.height, this.width];

        CreateCells();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // Create cells depend on level data
    void CreateCells()
    {
        // Determine how many units cell image is used
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

                // Determine which type of cell is created
                int cellToUse = MapFromGridStringToInteger(grid[j + this.width * i]);

                // Create cells 
                GameObject cell = Instantiate(cellTypes[cellToUse], position, Quaternion.identity);
                cell.transform.parent = this.transform;
                cell.name = "( " + i + ", " + j + " )";
                
                cell.GetComponent<Cell>().row = i;
                cell.GetComponent<Cell>().col = j;

                // Store cell
                allCells[i, j] = cell;
            }
        }

        // This row for creating new cells 
        //for (int i = 0; i < this.width; i++)
        //{
        //    allCells[this.height, i] = null;
        //}
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
            default: return Random.Range(0,4);
        }
    }


}
