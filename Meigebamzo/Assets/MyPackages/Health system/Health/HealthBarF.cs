using UnityEngine;

public abstract class HealthBarF : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void SetHealth(float hp);
    public abstract void SetMaxHealth(float value);
    public abstract void ReduceHP(float value);
}
