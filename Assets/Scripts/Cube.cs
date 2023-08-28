using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Cube : MonoBehaviour
{
   
    public Rectangle rectangle;
    public Gameplay gameplay;


    public Sprite[] spritesWithTntLogo;
    public Sprite[] spritesWithoutTntLogo;


    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        gameplay = FindObjectOfType<Gameplay>();

    }


    // Update is called once per frame
    void Update()
    {
        //ShowLogo();
    }


    void OnMouseDown()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        gameplay.DestroyCells(row, col);
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
                rectangle.allCells[row - 1, col].GetComponent<Obstacle>().Damage();
            }
        }

        if (row + 1 < rectangle.height && rectangle.allCells[row + 1, col] != null  )
        {
            if (rectangle.allCells[row + 1, col].tag == "box" || rectangle.allCells[row + 1, col].tag == "vase_01")
            {
                rectangle.allCells[row + 1, col].GetComponent<Obstacle>().Damage();
            }
        }

        if (col - 1 >= 0 && rectangle.allCells[row, col - 1] != null)
        {
            if (rectangle.allCells[row, col - 1].tag == "box" || rectangle.allCells[row, col - 1].tag == "vase_01")
            {
                rectangle.allCells[row, col - 1].GetComponent<Obstacle>().Damage();
            }
        }

        if (col + 1 < rectangle.width && rectangle.allCells[row, col + 1] != null)
        {
            if (rectangle.allCells[row, col + 1].tag == "box" || rectangle.allCells[row, col + 1].tag == "vase_01")
            {
                rectangle.allCells[row, col + 1].GetComponent<Obstacle>().Damage();
            }
        }

    }



    // If cube group is greater than 5 or equal to 5, display TNT logo on them
    void ShowLogo()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        // Find match for current cell
        gameplay.FindMatchesAt(rectangle.allCells, row, col);
        

        int tagToTnt = TagToLogo();
        
        
        if (gameplay.cellsToBeDestroyed.Count >= 5)
        {
            Debug.Log("b");
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
