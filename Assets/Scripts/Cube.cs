using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{




    // Start is called before the first frame update
    void Start()
    {
        
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


}
