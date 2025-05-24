using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int _sceneToLoad;
    [SerializeField] bool _loadOnStart;
    private void Start()
    {
       if(_loadOnStart) LoadSetScene();
    }
    public void LoadSetScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
