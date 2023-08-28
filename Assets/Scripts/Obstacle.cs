using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int health;

    public Rectangle rectangle;

    public Sprite brokenVase;

    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        
        if (tag == "box")
        {
            health = 1;
        }

        if (tag == "vase_01")
        {
            health = 2;
        }

        if (tag == "stone")
        {
            health = 5000;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        DestroyObstacle();

        if (tag == "vase_01")
        {
            isVaseDamaged();
        }
    }


    public void Damage()
    {
        health--;
    }


    public void TntDamage()
    {
        health = 0;

    }


    public void DestroyObstacle()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        if (health <= 0)
        {
            Destroy(gameObject);
            rectangle.allCells[row, col] = null;
        }
    }


    public void isVaseDamaged()
    {
        if (health == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenVase;
        }   
    }

}
