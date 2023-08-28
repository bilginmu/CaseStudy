using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public Rectangle rectangle;
    int damageId = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
    }


    // Update is called once per frame
    void Update()
    {
        CreateRandomFallingCells();
        damageId++;
    }


    // Check matches, if there is a match, destroy matched cells
    public void DestroyCells(List<GameObject> cellsToBeDestroyed, bool isCubeClicked)
    {

        // Check that adjacent cell is obstacle if user cliked a cube
        if (isCubeClicked)
        {
            CheckDestroyableObstacles(cellsToBeDestroyed);
        }
  
        

        // Destroy them
        for (int i = 0; i < cellsToBeDestroyed.Count; i++)
        {
            if (cellsToBeDestroyed[i] != null)
            {
                GameObject cell = cellsToBeDestroyed[i];

                int rowToBeDestroyed = cell.GetComponent<Cell>().row;
                int colToBeDestroyed = cell.GetComponent<Cell>().col;

                Destroy(cell);

                rectangle.allCells[rowToBeDestroyed, colToBeDestroyed] = null;
            }
            
        }        
    }



    // Check whether destroyabe obstacles is adjacent or not and give damage them
    void CheckDestroyableObstacles(List<GameObject> cellsToBeDestroyed)
    {
        for (int i = 0; i < cellsToBeDestroyed.Count; i++)
        {
            cellsToBeDestroyed[i].GetComponent<Cube>().IsAdjacentCellObstacle();
        }
    }


    // Check per frame and create cells if there is empty cell at top row
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
                
                cell.GetComponent<Cell>().row = topRow+1; // for checking empty below cell
                cell.GetComponent<Cell>().col = i;
                cell.GetComponent<Cell>().isFalling = true;
                
                cell.transform.parent = rectangle.transform;
                cell.name = "( " + topRow + ", " + i + " )";

                rectangle.allCells[topRow, i] = cell;
            }
        }
    }
}


