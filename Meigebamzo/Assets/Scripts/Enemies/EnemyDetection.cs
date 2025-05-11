using UnityEngine;
using UnityEngine.Events;

public class EnemyDetection : MonoBehaviour
{
    public UnityEvent<GameObject> OnEnemyDetected;
    public UnityEvent<GameObject> OnEnemyLeft;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnemyDetected?.Invoke(collision.gameObject.GetComponentInParent<Enemy>().gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnEnemyLeft?.Invoke(collision.gameObject.GetComponentInParent<Enemy>().gameObject);
    }
}
