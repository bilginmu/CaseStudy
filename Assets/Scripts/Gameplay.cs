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
       
    }


    // Find blasted cells and destroy them 
    public void DestroyCells(int row, int col)
    {
        FindMatchesAt(rectangle.allCells, row, col);
        
        Debug.Log(cellsToBeDestroyed.Count);

        if (cellsToBeDestroyed.Count > 1)
        {

            for (int i = 0; i < cellsToBeDestroyed.Count; i++)
            {
                GameObject cell = cellsToBeDestroyed[i];

                int rowToBeDestroyed = cell.GetComponent<Cell>().row;
                int colToBeDestroyed = cell.GetComponent<Cell>().col;
              
                Destroy(cell);
                
                Debug.Log("Destroyed " + "( " + rowToBeDestroyed + ", " + colToBeDestroyed + " )");

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

        Debug.Log("To be destroyed " + row + " " + col);
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

}


