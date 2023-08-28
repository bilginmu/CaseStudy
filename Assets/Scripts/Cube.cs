using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Profiling;
using UnityEngine;

public class Cube : MonoBehaviour
{
   
    public Rectangle rectangle;
    public Gameplay gameplay;

    public int damageId = 0;

    public Sprite[] spritesWithTntLogo;
    public Sprite[] spritesWithoutTntLogo;

    public List<GameObject> cellsToBeDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        gameplay = FindObjectOfType<Gameplay>();

        cellsToBeDestroyed = new List<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Cell>().isFalling)
        {
            int row = GetComponent<Cell>().row;
            int col = GetComponent<Cell>().col;

            FindMatchesAt(rectangle.allCells, row, col);
            ShowLogo();

            // Clear cells after finding matches
            ClearCellsToBeDestroyed();
        }

        damageId++;
        
    }


    void OnMouseDown()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        FindMatchesAt(rectangle.allCells, row, col);

        if (cellsToBeDestroyed.Count > 1)
        {
            gameplay.DestroyCells(cellsToBeDestroyed, true);
        }

        // Create TNT clicked position
        if (cellsToBeDestroyed.Count >= 5)
        {
            cellsToBeDestroyed.RemoveAt(0);

            // Position are determined from left to right
            Vector3 position = this.transform.position;

            // Determine which type of cell is created
            int cellToUse = rectangle.MapFromGridStringToInteger("t");

            // Create cells 
            GameObject tnt = Instantiate(rectangle.cellTypes[cellToUse], position, Quaternion.identity);

            tnt.transform.parent = rectangle.transform;
            tnt.name = "( " + row + ", " + col + " )";

            tnt.GetComponent<Cell>().row = row;
            tnt.GetComponent<Cell>().col = col;

            rectangle.allCells[row, col] = tnt;

            Destroy(gameObject);

        }
        ClearCellsToBeDestroyed();
    }


    // Check adjacent cell is obstacle 
    public void IsAdjacentCellObstacle()
    {
        

        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        if (row - 1 >= 0 && rectangle.allCells[row - 1, col] != null)
        {
            if (rectangle.allCells[row - 1, col].tag == "box" || rectangle.allCells[row - 1, col].tag == "vase_01")
            {
                rectangle.allCells[row - 1, col].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (row + 1 < rectangle.height && rectangle.allCells[row + 1, col] != null  )
        {
            if (rectangle.allCells[row + 1, col].tag == "box" || rectangle.allCells[row + 1, col].tag == "vase_01")
            {
                rectangle.allCells[row + 1, col].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (col - 1 >= 0 && rectangle.allCells[row, col - 1] != null)
        {
            if (rectangle.allCells[row, col - 1].tag == "box" || rectangle.allCells[row, col - 1].tag == "vase_01")
            {
                rectangle.allCells[row, col - 1].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (col + 1 < rectangle.width && rectangle.allCells[row, col + 1] != null)
        {
            if (rectangle.allCells[row, col + 1].tag == "box" || rectangle.allCells[row, col + 1].tag == "vase_01")
            {
                rectangle.allCells[row, col + 1].GetComponent<Obstacle>().Damage(damageId);
            }
        }

    }



    //Find adjacent cells with the same color, this method is based on Flood Fill algorithm.
    //Ref: https://en.wikipedia.org/wiki/Flood_fill
    public void FindMatchesAt(GameObject[,] allCells, int row, int col)
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
        if (col + 1 < rectangle.width)
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


    // Clear cells to be destroyed
    void ClearCellsToBeDestroyed()
    {
        for (int i = 0; i < cellsToBeDestroyed.Count; i++)
        {
            cellsToBeDestroyed[i].GetComponent<Cell>().isMatched = false;
        }
        cellsToBeDestroyed.Clear();
    }


    // If cube group is greater than 5 or equal to 5, display TNT logo on them
    void ShowLogo()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;        

        int tagToTnt = TagToLogo();

        if (cellsToBeDestroyed.Count >= 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spritesWithTntLogo[tagToTnt];
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spritesWithoutTntLogo[tagToTnt];
        }
       
    }


    // This is for logo with tnt and without tnt. Both of them are at the same index
    // for the same logo.
    private int TagToLogo()
    {
        switch (tag)
        {
            case "blue": return 0;
            case "green": return 1;
            case "yellow": return 2;
            case "red": return 3;
            default: return -1;
        }
    }
    

}
