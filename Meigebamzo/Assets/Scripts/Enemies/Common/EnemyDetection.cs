using UnityEngine;
using UnityEngine.Events;

public class EnemyDetection : MonoBehaviour
{
    public UnityEvent<GameObject> OnEnemyDetected;
    public UnityEvent<GameObject> OnEnemyLeft;
    private GameObject _detectedObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _detectedObject = collision.gameObject;
        if (_detectedObject.GetComponent<IDamagable>() != null) OnEnemyDetected?.Invoke(_detectedObject.GetComponent<IDamagable>().Transform.gameObject);
        else if (_detectedObject.GetComponentInParent<IDamagable>() != null) OnEnemyDetected?.Invoke(_detectedObject.GetComponentInParent<IDamagable>().Transform.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _detectedObject = collision.gameObject;
        if (_detectedObject.GetComponent<IDamagable>() != null) OnEnemyLeft?.Invoke(_detectedObject.GetComponent<IDamagable>().Transform.gameObject);
        else if (_detectedObject.GetComponentInParent<IDamagable>() != null) OnEnemyLeft?.Invoke(_detectedObject.GetComponentInParent<IDamagable>().Transform.gameObject);
    }
}
