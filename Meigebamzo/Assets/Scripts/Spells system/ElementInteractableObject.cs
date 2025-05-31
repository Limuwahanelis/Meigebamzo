using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementInteractableObject : MonoBehaviour, IDamagable
{
    public UnityEvent<BasicElement> OnElementInteracted;
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

    public void Kill(DamageInfo info)
    {
        OnDeath?.Invoke(this,info);
    }

    public void TakeDamage(DamageInfo info)
    {
        if (_elementsToInteractWith.Contains(info.basicElement.Element)) OnElementInteracted?.Invoke(info.basicElement);
    }

    private void Reset()
    {
        _elementalAffliction = GetComponent<ElementalAffliction>();
    }
}
