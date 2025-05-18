using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent OnLevelCompleted;
    [SerializeField] int _tasksToComplete;
    private int _completedTasks=0;
    public void CompleteTask()
    {
        _completedTasks++;
        if(_completedTasks == _tasksToComplete )
        {
            OnLevelCompleted?.Invoke();
        }
    }
}
