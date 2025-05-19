using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementInteractableObject : MonoBehaviour, IDamagable
{
    public UnityEvent<Elements.Element> OnElementInteracted;
    [SerializeField] List<Elements.Element> _elementsToInteractWith= new List<Elements.Element>();
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
        if (_elementsToInteractWith.Contains(info.element)) OnElementInteracted?.Invoke(info.element);
    }

    private void Reset()
    {
        _elementalAffliction = GetComponent<ElementalAffliction>();
    }
}
