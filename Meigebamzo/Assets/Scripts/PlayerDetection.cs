using UnityEngine;
using UnityEngine.Events;

public class PlayerDetection : MonoBehaviour
{
    public UnityEvent OnPlayerDetected;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerDetected?.Invoke();
    }
}
