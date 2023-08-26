using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;

    public bool isMatched = false;

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
        gameplay.DestroyCells(row, col);      
    }

    


    
}


