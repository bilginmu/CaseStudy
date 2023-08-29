using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    Rectangle rectangle;

    public float aspectRatio = 0.5625f ; // 9/16 
    public float padding = 2;

    void Start()
    {
        rectangle = FindObjectOfType<Rectangle>();
        SetCamera();
    }

    
    
    // Center camera with respect to rectangle included cells
    void SetCamera()
    {
        // Cube image pixels
        float pixelHeight = rectangle.cellPixelHeight;
        float pixelWidth = rectangle.cellPixelWidth;
        float unit = rectangle.unitPerPixel;

        // Cube area width and height
        float width = (float)rectangle.width * pixelWidth / unit;
        float height = (float)rectangle.height * pixelHeight / unit;   

        this.transform.position = new Vector3(width / 2f, height, -10);
        Camera.main.orthographicSize = 15;

    }
}
