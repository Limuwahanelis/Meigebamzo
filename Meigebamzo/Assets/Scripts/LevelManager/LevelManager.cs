using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent OnStartLevel;
    public UnityEvent OnLevelCompleted;
    [SerializeField] int _tasksToComplete;
    [SerializeField] Transform _cameraTran;
    [SerializeField] float _camMoveSpeed;
    private Camera _cam;
    private int _completedTasks=0;
    private void Start()
    {
        _cam = Camera.main;
    }
    public void CompleteTask()
    {
        _completedTasks++;
        if(_completedTasks == _tasksToComplete )
        {
            OnLevelCompleted?.Invoke();
        }
    }
    public void FailTask()
    {
        _completedTasks--;
    }
    public void StartMoveCam()
    {
        StartCoroutine(MoveCam());
    }
    public void StartLevel()
    {
        OnStartLevel?.Invoke();
    }
    private IEnumerator MoveCam()
    {
        Vector3 _cameraStartingPos = _cam.transform.position;
        float distance = Vector2.Distance(_cameraStartingPos, _cameraTran.position);
        float lerp = 0;
        while (lerp < 1)
        {
            Camera.main.transform.position = Vector3.Lerp(_cameraStartingPos, _cameraTran.position, lerp);
            lerp= lerp + (_camMoveSpeed * Time.deltaTime)/distance;
            yield return null;
        }
        Camera.main.transform.position = _cameraTran.position;
        StartLevel();
    }
}
