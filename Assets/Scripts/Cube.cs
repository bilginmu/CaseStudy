using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public Rectangle rectangle;
    public Gameplay gameplay;


    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        gameplay = FindObjectOfType<Gameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        GetComponent<Cell>().gameplay.DestroyCells(row, col);
    }


    public void IsAdjacentCellObstacle()
    {

        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;
        if (row - 1 >= 0 && rectangle.allCells[row - 1, col].tag == "box")
        {
            rectangle.allCells[row - 1, col].GetComponent<Box>().getDamaged();
        }

        if (row + 1 < rectangle.height && rectangle.allCells[row + 1, col].tag == "box")
        {
            rectangle.allCells[row + 1, col].GetComponent<Box>().getDamaged();
        }

        if (col - 1 >= 0 && rectangle.allCells[row, col - 1].tag == "box")
        {
            rectangle.allCells[row, col - 1].GetComponent<Box>().getDamaged();
        }

        if (col + 1 < rectangle.width && rectangle.allCells[row, col + 1].tag == "box")
        {
            
            rectangle.allCells[row, col + 1].GetComponent<Box>().getDamaged();
        }

    }

}
