using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElementalShield : MonoBehaviour
{
    public UnityEvent<ElementalShield> OnShieldDisabled;
    public List<ElementalSpell> ElementalSpellsToShieldFrom => _elementalSpellsToShieldFrom;
    [SerializeField]private List<ElementalSpell> _elementalSpellsToShieldFrom = new List<ElementalSpell>();
    private ElementalShieldLogic _elementalShieldLogic = new ElementalShieldLogic();
    private void Awake()
    {
        _elementalShieldLogic.OnShieldDisabled+=(ElementalShieldDisabled);
        SetUp(ElementalSpellsToShieldFrom);
    }
    public void SetUp(List<ElementalSpell> elementalSpells)
    {
        _elementalShieldLogic.SetUp(elementalSpells);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ElementalAffliction tmp= collision.attachedRigidbody.GetComponentInParent<ElementalAffliction>();
        if(tmp!=null)
        {
            tmp.AddElementsFromShield(_elementalShieldLogic);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ElementalAffliction tmp = collision.attachedRigidbody.GetComponentInParent<ElementalAffliction>();
        if (tmp != null)
        {
            tmp.RemoveElementsFromShield(_elementalShieldLogic);
        }
    }
    private void ElementalShieldDisabled(ElementalShieldLogic elementalShieldLogic)
    {
        OnShieldDisabled?.Invoke(this);
    }
    private void OnDisable()
    {
        _elementalShieldLogic.DisableShield();
    }
}
