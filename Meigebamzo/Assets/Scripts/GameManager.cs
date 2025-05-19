
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject toShow;
    [SerializeField] bool _showTutorial;
    [SerializeField] PlayerInputHandler _playerInput;
    [SerializeField] Transform _playerMainBody;
    [SerializeField] List<Transform> _spawnTrans = new List<Transform>();
    [SerializeField] List<Transform> _cameraTrans = new List<Transform>();
    [SerializeField] List<LevelManager> _levelManagers = new List<LevelManager>();
    public static int completedLevels = 0;

    public void Completelevel()
    {
        completedLevels++;
    }

    private void Start()
    {
        if(completedLevels==0)
        {
            if (_showTutorial)
            {
                _playerInput.SetEnabled(false);
                toShow.SetActive(true);
            }
            else
            {
                toShow.SetActive(false);
                _playerInput.SetEnabled(true);
            }
        }
        else
        {
            toShow.SetActive(false);
            _playerInput.SetEnabled(true);
            Camera.main.transform.position = _cameraTrans[completedLevels].position;
            _playerMainBody.transform.position = _spawnTrans[completedLevels].position;
            _levelManagers[completedLevels].StartLevel();
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
