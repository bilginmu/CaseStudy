using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int health;
    public int damageId = -1;

    public Rectangle rectangle;

    // Effects
    public GameObject[] boxDestroyEffect;

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


    // For one step, two different cube can give damage to obstacle but
    // this damage should be considered as 1 damage. Therefore, 
    // damageId is used. damageId changes for every step. If two
    // different cube give damage at one step, their damageId are the same.
    public void Damage(int damageId)
    {
        if (this.damageId != damageId)
        {
            health--;
            this.damageId = damageId;
        }
        
        //health--;
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

            if (tag == "box")
            {
                for (int i= 0; i < 3; i++)
                {
                    GameObject destroyEffect = Instantiate(boxDestroyEffect[i], this.transform.position, Quaternion.identity);
                    destroyEffect.transform.parent = rectangle.transform;
                }                
                

                
            }

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
