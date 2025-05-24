using UnityEngine;
using UnityEngine.Events;

public class TriggerDetection : MonoBehaviour
{
    public UnityEvent OnColliderDetected;
    public UnityEvent<Collider2D> OnColliderDetectedCol;
    [SerializeField] Collider2D _colliderToDetect;
    [SerializeField] bool _checkForSpecificCollider;
    [SerializeField] bool _disableDetectedGameObjectOnDetection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_checkForSpecificCollider)
        {
            if (collision == _colliderToDetect)
            {
                if (_disableDetectedGameObjectOnDetection) collision.gameObject.SetActive(false);
                OnColliderDetected?.Invoke();
                OnColliderDetectedCol?.Invoke(collision);
            }
        }
        OnColliderDetected?.Invoke();
        OnColliderDetectedCol?.Invoke(collision);
    }
}
