using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Profiling;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // TNT is created when five or greater cube are blasted
    public GameObject Tnt;

    // Rectangular grid which is parent of cubes, tnts, and obstacles 
    public GameObject rectangularGrid;

    public LevelGameplay gameplay;

    // At every step, cubes can give one damage. If multiple adjacent
    // cubes to a obstacle is blasted, this gives one damage.
    public int damageId = 0;

    // If this cube is matched, true otherwise false
    public bool isMatched = false;

    // Cubes with TNT logo and without TNT logo
    public Sprite[] spritesWithTntLogo;
    public Sprite[] spritesWithoutTntLogo;

    // List of cells to be destroyed
    public List<GameObject> cellsToBeDestroyed;


    // Start is called before the first frame update
    void Start()
    {
        // Game play 
        gameplay = FindObjectOfType<LevelGameplay>();
        rectangularGrid = gameplay.rectangularGrid;

        cellsToBeDestroyed = new List<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Cell>().isFalling)
        {
            int row = GetComponent<Cell>().row;
            int col = GetComponent<Cell>().col;

            Clear();

            FindMatchesAt(gameplay.allCells, row, col);
            ShowLogo();

            // Clear cells after finding matches
            Clear();
        }
    }


    void OnMouseDown()
    {  
        if (!GetComponent<Cell>().isFalling)
        {
            int row = GetComponent<Cell>().row;
            int col = GetComponent<Cell>().col;

            // Clear cells after finding matches
            Clear();

            FindMatchesAt(gameplay.allCells, row, col);

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

                // Create TNT at current position of the clicked cube
                GameObject tnt = Instantiate(Tnt, position, Quaternion.identity);

                tnt.transform.parent = rectangularGrid.transform;
                tnt.name = "( " + row + ", " + col + " )";

                tnt.GetComponent<Cell>().row = row;
                tnt.GetComponent<Cell>().col = col;

                gameplay.allCells[row, col] = tnt;

                Destroy(gameObject);

            }
            Clear();
        }          
    }


    // Check adjacent cell is obstacle 
    public void GiveDamageToObstacle(int damageId)
    {

        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        if (row - 1 >= 0 && gameplay.allCells[row - 1, col] != null)
        {
            if (gameplay.allCells[row - 1, col].tag == "box" || gameplay.allCells[row - 1, col].tag == "vase_01")
            {
                gameplay.allCells[row - 1, col].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (row + 1 < gameplay.height && gameplay.allCells[row + 1, col] != null)
        {
            if (gameplay.allCells[row + 1, col].tag == "box" || gameplay.allCells[row + 1, col].tag == "vase_01")
            {
                gameplay.allCells[row + 1, col].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (col - 1 >= 0 && gameplay.allCells[row, col - 1] != null)
        {
            if (gameplay.allCells[row, col - 1].tag == "box" || gameplay.allCells[row, col - 1].tag == "vase_01")
            {
                gameplay.allCells[row, col - 1].GetComponent<Obstacle>().Damage(damageId);
            }
        }

        if (col + 1 < gameplay.width && gameplay.allCells[row, col + 1] != null)
        {
            if (gameplay.allCells[row, col + 1].tag == "box" || gameplay.allCells[row, col + 1].tag == "vase_01")
            {
                gameplay.allCells[row, col + 1].GetComponent<Obstacle>().Damage(damageId);
            }
        }

    }



    //Find adjacent cells with the same color, this method is based on Flood Fill algorithm.
    //Ref: https://en.wikipedia.org/wiki/Flood_fill
    public void FindMatchesAt(GameObject[,] allCells, int row, int col)
    {
        if (row == gameplay.height)
        {
            return;
        }

        GameObject cell = allCells[row, col];

        // Cell can be null because of falling down
        if (cell != null && cell.GetComponent<Cube>() != null && !cell.GetComponent<Cell>().isFalling)
        {
            cell.GetComponent<Cube>().isMatched = true;
            cellsToBeDestroyed.Add(cell);

            // Check left cell
            if (col - 1 >= 0)
            {
                GameObject leftCell = allCells[row, col - 1];
                if (leftCell != null && leftCell.GetComponent<Cube>() != null && !leftCell.GetComponent<Cube>().isMatched && leftCell.tag == cell.tag)
                {
                    FindMatchesAt(allCells, row, col - 1);
                }

            }

            // Check right cell
            if (col + 1 < gameplay.width)
            {
                GameObject rightCell = allCells[row, col + 1];
                if (rightCell != null && rightCell.GetComponent<Cube>() != null && !rightCell.GetComponent<Cube>().isMatched && rightCell.tag == cell.tag)
                {
                    FindMatchesAt(allCells, row, col + 1);
                }
            }

            // Check down cell
            if (row - 1 >= 0)
            {
                GameObject downCell = allCells[row - 1, col];

                if (downCell != null && downCell.GetComponent<Cube>() != null && !downCell.GetComponent<Cube>().isMatched && downCell.tag == cell.tag)
                {
                    FindMatchesAt(allCells, row - 1, col);
                }
            }

            // Check up cell
            if (row + 1 < gameplay.height)
            {
                GameObject upCell = allCells[row + 1, col];

                if (upCell != null && upCell.GetComponent<Cube>() != null && !upCell.GetComponent<Cube>().isMatched && upCell.tag == cell.tag)
                {
                    FindMatchesAt(allCells, row + 1, col);
                }
            }


        }
    }


    // Clear cells to be destroyed
    void Clear()
    {
        for (int i = 0; i < cellsToBeDestroyed.Count; i++)
        {
            cellsToBeDestroyed[i].GetComponent<Cube>().isMatched = false;
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
