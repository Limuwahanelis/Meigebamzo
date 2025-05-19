using UnityEngine;
using UnityEngine.Events;

public class ColliderDetection : MonoBehaviour
{
    public UnityEvent OnColliderDetected;
    [SerializeField] Collider2D _colliderToDetect;
    [SerializeField] bool _disableGameObjectOnDetection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == _colliderToDetect)
        {
            if (_disableGameObjectOnDetection) collision.gameObject.SetActive(false);
            OnColliderDetected?.Invoke();
        }
    }
}
