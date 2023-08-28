using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Tnt : MonoBehaviour
{
    private Rectangle rectangle;
    private Gameplay gameplay;

    
    
    public List<GameObject> cellsToBeDestroyed;
    public bool isDestroyed = false;


    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        gameplay = FindAnyObjectByType<Gameplay>();

        cellsToBeDestroyed = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseDown()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        DestroyCells(row, col);

        gameplay.DestroyCells(cellsToBeDestroyed, false);

        cellsToBeDestroyed.Clear();


    }



    public void DestroyCells(int row, int col)
    {

        bool isAdjacentCellTnt = IsAdjacentCellTnt();

        int destroySize = 2;

        if (isAdjacentCellTnt)
        {
            destroySize = 3;
        }


        for (int i=-destroySize; i <= destroySize; i++)
        {
            for (int j = -destroySize; j <= destroySize; j++)
            {
                if (row + i >= 0 && row + i < rectangle.height && col + j >= 0 && col + j < rectangle.width)
                {
                    GameObject cell = rectangle.allCells[row + i, col + j];

                    if (!cell.GetComponent<Cell>().isDestroyed)
                    {
                        cellsToBeDestroyed.Add(cell);
                        cell.GetComponent<Cell>().isDestroyed = true;


                        if (cell.tag == "tnt")
                        {
                            DestroyCells(cell.GetComponent<Cell>().row, cell.GetComponent<Cell>().col);
                        }
                    }
                }

            }
        }

    }

    
    private bool IsAdjacentCellTnt()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        // If down cell is valid and it is a tnt
        if (row - 1 >= 0 && rectangle.allCells[row - 1, col] != null && rectangle.allCells[row - 1, col].tag == "tnt")
        {
            return true;
        }

        // If up cell is valid and it is a tnt
        if (row + 1 < rectangle.height && rectangle.allCells[row + 1, col] != null && rectangle.allCells[row + 1, col].tag == "tnt")
        {
            return true;
        }

        // If left cell is valid and it is a tnt
        if (col - 1 < rectangle.height && rectangle.allCells[row, col - 1] != null && rectangle.allCells[row, col - 1].tag == "tnt")
        {
            return true;
        }

        // If right cell is valid and it is a tnt
        if (col + 1 < rectangle.height && rectangle.allCells[row, col + 1] != null && rectangle.allCells[row, col + 1].tag == "tnt")
        {
            return true;
        }

        return false;

    }


    // If there is tnt cell in to be destroyed cell, add them to destroyed list
    void CheckTnt()
    {
        for (int i= 0; i < cellsToBeDestroyed.Count; i++ )
        {
            if (cellsToBeDestroyed[i].tag == "tnt")
            {

            }
        }
    }


}
