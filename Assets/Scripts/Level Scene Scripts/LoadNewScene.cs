using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    public GameObject celebrationEffect;


    public void LoadMainScene()
    {
        SceneManager.LoadScene("main_scene");
    }


}
