using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// This class represents attributes that every cell has. 
// Also almost every cell is falling freely down. Free fall
// mechanism is implemented in tihs class.
public class Cell : MonoBehaviour
{
    // Index of cell in the rectangle grid
    public int row;
    public int col;

    // If cell is falling and it is not arrived to below cell, true otherwise false
    public bool isFalling = false;
    
    // If a tnt destroys cell, true otherwise false
    public bool isDestroyed = false;

    // Free fall parameters
    private float g = 9.81f; // gravity
    private float scale = 450.0f; // gravity scale

    // To get other cells information 
    private LevelGameplay gameplay;


    // Start is called before the first frame update
    void Start()
    {
        // To get other cells information
        gameplay = FindObjectOfType<LevelGameplay>();
    }


    // Update is called once per frame
    void Update()
    {
        
        // Cells at upper row has greater order than bottom ones
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = row;

        //Box and stones are not falling
        if (tag != "box" && tag != "stone")
        {
            FallDown();
        }
    }


    // If down cell is destroyed, this cell is falling state
    void FallDown()
    {

        // if down cell is emtpy, this cell should fall down
        if (!isFalling && row > 0 && gameplay.allCells[row - 1, col] == null)
        {
            isFalling = true;
        }

        // If cell is falling, it is freely falling until cell is positioned at below cell
        if (isFalling)
        {
            FreeFall();

            Vector2 downCellPosition = GetPositionFrom(row - 1, col);

            // If 10 pixel remains to get desired position, go there
            if (Mathf.Abs(transform.position.y - downCellPosition[1]) < 0.1 || transform.position.y - downCellPosition[1] < 0)
            { 
                transform.position = downCellPosition;
                
                // When random falling cells are created, their row are the same with height.
                // But height index for allCell array is not available. Therefore allCells[row, col] 
                // gives outofindex error.
                if (row != gameplay.height)
                {
                    gameplay.allCells[row - 1, col] = gameplay.allCells[row, col];
                    gameplay.allCells[row, col] = null;
                    // If upper cell is null, it could not has cell component.
                    if (gameplay.allCells[row - 1, col] != null)
                    {
                        gameplay.allCells[row - 1, col].GetComponent<Cell>().name = "( " + (row - 1) + ", " + col + " )";
                    }
                }

                row = row - 1;

                isFalling = false;
            }
        }
    }


    // Free fall model
    void FreeFall()
    {
        // Same x position
        float x = transform.position.x;

        // Displacement is calculated based on free fall equation 
        float displacement = (float)(1.0 / 2.0) * g * scale * Time.deltaTime * Time.deltaTime;
        float y = transform.position.y - displacement;

        // Same z position
        float z = transform.position.z;

        // Fall down
        transform.position = new Vector3(x, y, z);
    }


    // Calculate position from row and column of the cell.
    Vector2 GetPositionFrom(int row, int col)
    {
        Vector2 position = new Vector2(gameplay.leftOffset + col * gameplay.cellWidth, gameplay.bottomOffset + row * gameplay.cellHeight);
        return position;
    }
}