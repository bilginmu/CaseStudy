using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public Rectangle rectangle;

    private List<GameObject> cellsToBeDestroyed;
    
    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        cellsToBeDestroyed = new List<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {
        CreateRandomFallingCells();
    }


    // Check matches, if there is a match, destroy matched cells
    public void DestroyCells(int row, int col)
    {
        FindMatchesAt(rectangle.allCells, row, col);
        

        // If clicked cell has at least 1 adjacent with the same color, destroy them
        // otherwise clear isMatched
        if (cellsToBeDestroyed.Count > 1)
        {

            for (int i = 0; i < cellsToBeDestroyed.Count; i++)
            {
                GameObject cell = cellsToBeDestroyed[i];

                int rowToBeDestroyed = cell.GetComponent<Cell>().row;
                int colToBeDestroyed = cell.GetComponent<Cell>().col;
              
                Destroy(cell);
                
                rectangle.allCells[rowToBeDestroyed, colToBeDestroyed] = null;
            }

        }
        else
        {
            rectangle.allCells[row, col].GetComponent<Cell>().isMatched = false;
        }

        cellsToBeDestroyed.Clear(); 
    }


    // Find adjacent cells with the same color, this method is based on Flood Fill algorithm.
    // Ref: https://en.wikipedia.org/wiki/Flood_fill
    void FindMatchesAt(GameObject[,] allCells, int row, int col)
    {
        GameObject cell = allCells[row, col];
        cell.GetComponent<Cell>().isMatched = true;
        cellsToBeDestroyed.Add(cell);
        
        // Check left cell
        if (col - 1 >= 0)
        {
            GameObject leftCell = allCells[row, col - 1];
            if (leftCell != null && !leftCell.GetComponent<Cell>().isMatched && leftCell.tag == cell.tag)
            {
                FindMatchesAt(allCells, row, col - 1);
            }

        }
        
        // Check right cell
        if (col + 1 <  rectangle.width)
        {
            GameObject rightCell = allCells[row, col + 1];
            if (rightCell != null && !rightCell.GetComponent<Cell>().isMatched && rightCell.tag == cell.tag)
            {
                FindMatchesAt(allCells, row, col + 1);
            }
        }

        // Check down cell
        if (row - 1 >= 0)
        {
            GameObject downCell = allCells[row - 1, col];

            if (downCell != null && !downCell.GetComponent<Cell>().isMatched && downCell.tag == cell.tag)
            {
                FindMatchesAt(allCells, row - 1, col);
            }
        }

        // Check up cell
        if (row + 1 < rectangle.height)
        {
            GameObject upCell = allCells[row + 1, col];

            if (upCell != null && !upCell.GetComponent<Cell>().isMatched && upCell.tag == cell.tag)
            {
                FindMatchesAt(allCells, row + 1, col);
            }
        }
    }


    // Check per frame and create cell if there is empty cell at top row
    void CreateRandomFallingCells()
    {
        int topRow = rectangle.height - 1;
        for (int i = 0; i < rectangle.width; i++)
        {
            if (rectangle.allCells[topRow, i] == null)
            {
                // Determine how many units cell image is used
                float heightUnit = (float)rectangle.cellPixelHeight / rectangle.unitPerPixel;
                float widthUnit = (float)rectangle.cellPixelWidth / rectangle.unitPerPixel;

                // Cell will be created at the top of rectangle
                Vector2 position = new Vector2(i* widthUnit, (topRow+1)* heightUnit);

                int cellType = Random.Range(0, 4);
                GameObject cell = Instantiate(rectangle.cellTypes[cellType], position, Quaternion.identity);
                
                cell.GetComponent<Cell>().row = topRow+1;
                cell.GetComponent<Cell>().col = i;
                cell.GetComponent<Cell>().isFalling = true;

                rectangle.allCells[topRow, i] = cell;
            }
        }

    }
}


