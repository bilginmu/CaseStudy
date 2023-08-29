using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    // Destroy effects
    public GameObject[] boxDestroyEffect;
    public GameObject blueCubeDestroyEffect;
    public GameObject redCubeDestroyEffect;
    public GameObject greenCubeDestroyEffect;
    public GameObject yellowCubeDestroyEffect;


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

                ShowDestroyEffects(cell);

                Destroy(cell);

                rectangle.allCells[rowToBeDestroyed, colToBeDestroyed] = null;
            }
            
        }        
    }



    // Check whether destroyabe obstacles is adjacent or not
    // If it is an obstacle, give damage them
    void CheckDestroyableObstacles(List<GameObject> cellsToBeDestroyed)
    {
        for (int i = 0; i < cellsToBeDestroyed.Count; i++)
        {
            cellsToBeDestroyed[i].GetComponent<Cube>().GiveDamageToObstacle(damageId);
        }

        damageId++;
    }


    // When a cube destroyed, create falling cells instead of destroyed ones
    void CreateRandomFallingCells()
    {
        int topRow = rectangle.height - 1;
        for (int i = 0; i < rectangle.width; i++)
        {
            // If top row is null, we need to create new ones
            if (rectangle.allCells[topRow, i] == null)
            {
                // Determine how many units cell uses
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


    void ShowDestroyEffects(GameObject cell)
    {
      
        if (cell.tag == "box")
        {
            // Show 3 different particle
            for (int i = 0; i < 3; i++)
            {
                GameObject destroyEffect = Instantiate(boxDestroyEffect[i], cell.transform.position, Quaternion.identity);
                destroyEffect.transform.parent = rectangle.transform;
            }
        }

        if (cell.tag == "blue")
        {
            GameObject destroyEffect = Instantiate(blueCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangle.transform;
        }

        if (cell.tag == "red")
        {
            GameObject destroyEffect = Instantiate(redCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangle.transform;
        }

        if (cell.tag == "green")
        {
            GameObject destroyEffect = Instantiate(greenCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangle.transform;
        }

        if (cell.tag == "yellow")
        {
            GameObject destroyEffect = Instantiate(yellowCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangle.transform;
        }
    }
}


