using UnityEngine;
using UnityEngine.Events;

public class ElementInteractableObject : MonoBehaviour, IDamagable
{
    public UnityEvent OnElementInteracted;
    [SerializeField] Elements.Element _elementToInteractWith;
    [SerializeField] ElementalAffliction _elementalAffliction;
    public Transform Transform => transform;
    public ElementalAffliction ElementalAffliction => _elementalAffliction;

    public Transform MainBody => transform;

    public event IDamagable.OnDeathEventHandler OnDeath;

    
    private void Awake()
    {
        //_elementalAffliction=GetComponent<ElementalAffliction>()
    }

    public void Kill()
    {
        OnDeath?.Invoke(this);
    }

    public void TakeDamage(DamageInfo info)
    {
        if (info.element == _elementToInteractWith) OnElementInteracted?.Invoke();
    }

    private void Reset()
    {
        _elementalAffliction = GetComponent<ElementalAffliction>();
    }
}
