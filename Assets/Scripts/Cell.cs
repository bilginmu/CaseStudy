using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;

    public bool isMatched = false;
    public bool isFalling = false;

    // Free fall parameters
    private float g = 9.81f; // gravity
    private float scale = 400.0f; // gravity scale

    // These parameters used for calculate position from (row,col)
    private int cellPixelHeight = 162;
    private int cellPixelWidth = 142;
    private int unitPerPixel = 100;

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
        FallDown();
        //StartCoroutine(FallDownCo());
    }

   
    void OnMouseDown()
    {
        gameplay.DestroyCells(row, col);      
    }

  


    // If down cell is destroyed, this cell is falling state
    void FallDown()
    {

        // if down cell is emtpy, this cell should fall down
        if (row > 0 && rectangle.allCells[row - 1, col] == null)
        {
            // This cell is falling down
            rectangle.allCells[row - 1, col] = rectangle.allCells[row, col];
            rectangle.allCells[row - 1, col].GetComponent<Cell>().name = "( " + (row-1) + ", " + col + " )";
            rectangle.allCells[row, col] = null;
            isFalling = true;
        
        }

        // If cell is falling, free fall until cell is positioned at below cell
        if (isFalling)
        {
            FreeFall();

            Vector2 downCellPosition = GetPositionFrom(row - 1, col);

            // If 20 pixel remains to get desired position, go there
            if (Mathf.Abs(transform.position.y - downCellPosition[1]) < 0.2 || transform.position.y - downCellPosition[1] < 0)
            {
                Debug.Log("Musti bug2.");
                transform.position = downCellPosition;
                isFalling = false;
                row = row - 1;
            }
        }
            

            


        
    }


    void FreeFall()
    {
        // Same x position
        float x = transform.position.x; 

        // Displacement is calculated based on free fall equation 
        float displacement = (float) (1.0 / 2.0) * g * scale * Time.deltaTime * Time.deltaTime;
        float y = transform.position.y - displacement;

        // Same y position
        float z = transform.position.z;

        //Debug.Log("Displacement : " + displacement);
        //Debug.Log("Delta time   : " + Time.deltaTime);

        // Fall down
        transform.position = new Vector3(x , y, z);        
    }
    


    Vector2 GetPositionFrom(int row, int col)
    {
        float heightUnit = (float)cellPixelHeight / unitPerPixel;
        float widthUnit = (float)cellPixelWidth / unitPerPixel;

        Vector2 position = new Vector2(col * widthUnit, row * heightUnit);
        return position; 
    }


    
}


