using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void Start()
    {
        LoadNextScene();
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
