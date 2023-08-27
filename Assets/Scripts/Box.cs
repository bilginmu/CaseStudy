using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int damage = 1;

    public Rectangle rectangle;

    // Start is called before the first frame update
    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBox();
    }

    public void getDamaged()
    {
        damage--;
    }


    public void DestroyBox()
    {
        int row = GetComponent<Cell>().row;
        int col = GetComponent<Cell>().col;

        if (damage <= 0)
        {
            Destroy(gameObject);
            rectangle.allCells[row, col] = null;
        }
    }

}
