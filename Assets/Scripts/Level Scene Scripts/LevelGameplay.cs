using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Drawing;

public class LevelGameplay : MonoBehaviour
{
    //
    // ANIMATION 
    //
    public Animator celebration;

    //
    // DESTROY EFFECTs
    //
    public GameObject[] boxDestroyEffect;
    public GameObject[] vaseDestroyEffect;
    public GameObject[] stoneDestroyEffect;
    public GameObject blueCubeDestroyEffect;
    public GameObject greenCubeDestroyEffect;
    public GameObject redCubeDestroyEffect;
    public GameObject yellowCubeDestroyEffect;


    //
    // TOP UI ATTRIBUTEs
    //
    // Task Text
    public TMP_Text boxTaskText;
    public TMP_Text stoneTaskText;
    public TMP_Text vaseTaskText;

    // Task image
    public GameObject boxTaskObject;
    public GameObject stoneTaskObject;
    public GameObject vaseTaskObject;

    // Task tick image
    public GameObject boxTaskTick;
    public GameObject stoneTaskTick;
    public GameObject vaseTaskTick;

    // Move count task
    public TMP_Text moveCountText;


    // 
    // GAME ATTRIBUTEs
    // 
    // Cell types which are four cubes, stone, vase, box and tnt
    public GameObject[] cellTypes;

    // Cells that contain cubes
    public GameObject[,] allCells;

    // Parent gameobject of cells
    public GameObject rectangularGrid;

    // These indicates how many unit will be destroyed from which obstacle
    public int boxTask = 0;
    public int vaseTask = 0;
    public int stoneTask = 0;
    
    // If all related obstacle is destroyed, true otherwise false
    private bool isBoxTaskFinished = false;
    private bool isVaseTaskFinished = false;
    private bool isStoneTaskFinished = false;

    // Cube, TNT, or obstacle size
    public float cellHeight; 
    public float cellWidth;  
    public float leftOffset;
    public float bottomOffset;

    // Level information
    public int height;
    public int width;
    public int moveCount;
    private string[] grid;

    private int damageId = 0 ;
    

    [SerializeField]private int levelFinished = 0;

    // Level information in the json file
    private JsonFile levelInfo;

    Scene scene;

    // Start is called before the first frame update
    void Start()
    {   
        // Current scene
        scene = SceneManager.GetActiveScene();

        // Get json file information
        levelInfo = GetComponent<JsonFile>();
        levelInfo.GetJsonFileInfo(scene.name);

        // Level information
        height = levelInfo.GetHeight();
        width = levelInfo.GetWidth();
        grid = levelInfo.GetGrid();
        moveCount = levelInfo.GetMoveCount();

        // Calculate left bottom corner of rectangle grid centralize 
        float cameraHeight = 30f;
        float aspectRatio = 9f / 16f;
        float cameraWidth = cameraHeight * aspectRatio;

        // Middle of screen is (0,0)
        leftOffset = -(width - 1) * cellWidth / 2f;//-(cameraWidth-width*cellWidth )/ 2f;
        bottomOffset = -12f;// (cameraHeight- height*cellHeight) / 2f ;

        // Create cells with current level information
        allCells = new GameObject[height, width];

        CreateCells();
    }


    // Update is called once per frame
    void Update()
    {
        ShowTasks();
        ShowMoveCount();
        CreateRandomFallingCells();
        DetermineEndGameConditions();
        
        //if(levelFinished != 0)
        //{
        //    rectangularGrid.transform.position = new Vector2(50f, 50f);
        //    leftOffset = 50f;

        //}
    }


    // Create cells depend on level
    void CreateCells()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // Position are determined from left to right
                Vector2 position = new Vector2(leftOffset + j * cellWidth, bottomOffset +  i * cellHeight);

                // Determine which type of cell is created
                int cellToUse = MapFromGridStringToInteger(grid[j + width * i]);
               
                // Box, stone, and vase tasks are stored
                SetTasks(cellToUse);

                // Create cells 
                GameObject cell = Instantiate(cellTypes[cellToUse], position, Quaternion.identity);

                // Set cell properties
                cell.transform.parent = transform;
                cell.name = "( " + i + ", " + j + " )";

                cell.GetComponent<Cell>().row = i;
                cell.GetComponent<Cell>().col = j;

                // Store cell
                allCells[i, j] = cell;
            }
        }
    }


    // Map from grid string in json to tag
    public int MapFromGridStringToInteger(string gridString)
    {
        switch (gridString)
        {
            case "b": return 0;
            case "g": return 1;
            case "r": return 2;
            case "y": return 3;
            case "bo": return 4;
            case "t": return 5;
            case "s": return 6;
            case "v": return 7;
            default: return Random.Range(0, 4);
        }
    }


    // Count how many obstacles is in json file
    private void SetTasks(int task)
    {
        if (task == 4)
        {
            boxTask++;
        }

        if (task == 6)
        {
            stoneTask++;
        }

        if (task == 7)
        {
            vaseTask++;
        }
    }


    // Show tasks upper left of the screen
    private void ShowTasks()
    {
        if (boxTask > 0 && boxTask < 50)
        {
            boxTaskObject.SetActive(true);
            boxTaskText.text = boxTask.ToString();
        }
        if (stoneTask > 0 && stoneTask < 50)
        {
            stoneTaskObject.SetActive(true);
            stoneTaskText.text = stoneTask.ToString();
        }
        if (vaseTask > 0 && vaseTask < 50)
        {
            vaseTaskObject.SetActive(true);
            vaseTaskText.text = vaseTask.ToString();
        }

        if (boxTask <= 0)
        {
            isBoxTaskFinished = true;
        }
        if (stoneTask <= 0)
        {
            isStoneTaskFinished = true;
        }
        if (vaseTask <= 0)
        {
            isVaseTaskFinished = true;
        }


        if (isBoxTaskFinished)
        {
            boxTaskText.gameObject.SetActive(false);
            boxTaskTick.SetActive(true);
        }

        if (isStoneTaskFinished)
        {
            stoneTaskText.gameObject.SetActive(false);
            stoneTaskTick.SetActive(true);
        }

        if (isVaseTaskFinished)
        {
            vaseTaskText.gameObject.SetActive(false);
            vaseTaskTick.SetActive(true);
        }

    }

    // Show remaining move count at the right upper corner
    void ShowMoveCount()
    {
        moveCountText.text = moveCount.ToString();
    }


    // Determine that game is finished because of what
    void DetermineEndGameConditions()
    {
        // Game is finished because of move count
        if (moveCount == 0)
        {
            levelFinished = 2;
            PlayerPrefs.SetInt(scene.name, levelFinished);
            celebration.SetInteger("levelFinished", levelFinished);
        }

        // Game is finished thanks to that tasks are finished
        if (isVaseTaskFinished && isBoxTaskFinished && isStoneTaskFinished)
        {
            levelFinished = 1;
            PlayerPrefs.SetInt(scene.name, levelFinished);
            celebration.SetInteger("levelFinished", levelFinished);

        }
    }


    // Destroy cells and check whether obstacle is adjacent to destroyed cells or not
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

                // If new created cell row is destroyed
                if (rowToBeDestroyed == height)
                {
                    rowToBeDestroyed--;
                }

                ShowDestroyEffects(cell);

                Destroy(cell);

                allCells[rowToBeDestroyed, colToBeDestroyed] = null;
            }
        }
        moveCount--;
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


    // Show destroy effects
    void ShowDestroyEffects(GameObject cell)
    {

        if (cell.tag == "box")
        {
            // Show 3 different particle
            for (int i = 0; i < 3; i++)
            {
                GameObject destroyEffect = Instantiate(boxDestroyEffect[i], cell.transform.position, Quaternion.identity);
                destroyEffect.transform.parent = rectangularGrid.transform;
            }

            boxTask--;
        }

        if (cell.tag == "blue")
        {
            GameObject destroyEffect = Instantiate(blueCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangularGrid.transform;
        }

        if (cell.tag == "red")
        {
            GameObject destroyEffect = Instantiate(redCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangularGrid.transform;
        }

        if (cell.tag == "green")
        {
            GameObject destroyEffect = Instantiate(greenCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangularGrid.transform;
        }

        if (cell.tag == "yellow")
        {
            GameObject destroyEffect = Instantiate(yellowCubeDestroyEffect, cell.transform.position, Quaternion.identity);
            destroyEffect.transform.parent = rectangularGrid.transform;
        }

        if (cell.tag == "vase_01")
        {
            // Show 3 different particle
            for (int i = 0; i < 3; i++)
            {
                GameObject destroyEffect = Instantiate(vaseDestroyEffect[i], cell.transform.position, Quaternion.identity);
                destroyEffect.transform.parent = rectangularGrid.transform;
            }

            vaseTask--;
        }

        if (cell.tag == "stone")
        {
            // Show 3 different particle
            for (int i = 0; i < 3; i++)
            {
                GameObject destroyEffect = Instantiate(stoneDestroyEffect[i], cell.transform.position, Quaternion.identity);
                destroyEffect.transform.parent = rectangularGrid.transform;
            }

            stoneTask--;
        }
    }


    // When a cube destroyed, create falling cells instead of destroyed ones
    void CreateRandomFallingCells()
    {
        int topRow = height - 1;
        for (int i = 0; i < width; i++)
        {
            // If top row is null, we need to create new ones
            if (allCells[topRow, i] == null)
            {

                // Cell will be created at the top of rectangle
                Vector2 position = new Vector2(leftOffset + i * cellWidth, bottomOffset + (topRow + 1) * cellHeight);

                int cellType = Random.Range(0, 4);
                GameObject cell = Instantiate(cellTypes[cellType], position, Quaternion.identity);

                cell.GetComponent<Cell>().row = topRow + 1; // for checking empty below cell
                cell.GetComponent<Cell>().col = i;
                cell.GetComponent<Cell>().isFalling = true;

                cell.transform.parent = rectangularGrid.transform;
                cell.name = "( " + topRow + ", " + i + " )";

                allCells[topRow, i] = cell;
            }
        }
    }

}

