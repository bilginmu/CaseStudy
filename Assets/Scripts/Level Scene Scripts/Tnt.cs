using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Tnt : MonoBehaviour
{
    private GameObject rectangularGrid;
    private LevelGameplay gameplay;

    public List<GameObject> cellsToBeDestroyed;
 


    // Start is called before the first frame update
    void Start()
    {
        gameplay = FindAnyObjectByType<LevelGameplay>();

        rectangularGrid = gameplay.rectangularGrid;

        cellsToBeDestroyed = new List<GameObject>();
    }


    void OnMouseDown()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        DestroyCells(row, col);

        gameplay.DestroyCells(cellsToBeDestroyed, false);

        cellsToBeDestroyed.Clear();
    }


    // Explode TNT When tapped or damaged by another TNT
    public void DestroyCells(int row, int col)
    {
        bool isAdjacentCellTnt = IsAdjacentCellTnt(row, col);

        int destroySize = 2;

        if (isAdjacentCellTnt)
        {
            destroySize = 3;
        }

        for (int i = -destroySize; i <= destroySize; i++)
        {
            for (int j = -destroySize; j <= destroySize; j++)
            {
                if (row + i >= 0 && row + i < gameplay.height && col + j >= 0 && col + j < gameplay.width)
                {

                    GameObject cell = gameplay.allCells[row + i, col + j];

                    // If the cell has already been destroyed, do not add it to the list to avoid duplication
                    if (cell != null && !cell.GetComponent<Cell>().isDestroyed)
                    {
                        cellsToBeDestroyed.Add(cell);
                        cell.GetComponent<Cell>().isDestroyed = true;

                        // When a TNT is damaged by another TNT and it is not adjacent to current TNT, it will explode
                        // If a TNT is adjacent to another TNT, destroy size will be 7x7.
                        if (cell.tag == "tnt" && !IsAdjacent(row, col, cell.GetComponent<Cell>().row, cell.GetComponent<Cell>().col))
                        {
                            DestroyCells(cell.GetComponent<Cell>().row, cell.GetComponent<Cell>().col);
                        }
                    }
                }
            }
        }
    }


    // If adjacent cell is tnt, it trigers adjacent tnt.
    private bool IsAdjacentCellTnt(int row, int col)
    {

        // If down cell is valid and it is a tnt
        if (row - 1 >= 0 && gameplay.allCells[row - 1, col] != null && gameplay.allCells[row - 1, col].tag == "tnt")
        {
            return true;
        }

        // If up cell is valid and it is a tnt
        if (row + 1 < gameplay.height && gameplay.allCells[row + 1, col] != null && gameplay.allCells[row + 1, col].tag == "tnt")
        {
            return true;
        }

        // If left cell is valid and it is a tnt
        if (col - 1 >= 0 && gameplay.allCells[row, col - 1] != null && gameplay.allCells[row, col - 1].tag == "tnt")
        {
            return true;
        }

        // If right cell is valid and it is a tnt
        if (col + 1 < gameplay.width && gameplay.allCells[row, col + 1] != null && gameplay.allCells[row, col + 1].tag == "tnt")
        {
            return true;
        }

        return false;

    }


    // Given row and col is adjacent to adjRow and adjCol
    bool IsAdjacent(int row, int col, int adjRow, int adjCol)
    {

        if (row == adjRow + 1 && col == adjCol)
        {
            return true;
        }

        if (row == adjRow - 1 && col == adjCol)
        {
            return true;
        }

        if (row == adjRow && col == adjCol + 1)
        {
            return true;
        }

        if (row == adjRow && col == adjCol - 1)
        {
            return true;
        }

        return false;

    }
}
