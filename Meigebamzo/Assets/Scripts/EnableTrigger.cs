using UnityEngine;

public class EnableTrigger : MonoBehaviour
{
    [SerializeField] Collider2D _trigger;
    public void Enable()
    {
        _trigger.enabled = true;
    }
}
