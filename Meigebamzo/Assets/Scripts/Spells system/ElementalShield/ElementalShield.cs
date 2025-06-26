using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElementalShield : MonoBehaviour
{
    public List<ElementalSpell> ElementalSpellsToShieldFrom => _elementalSpellsToShieldFrom;
    private List<Elements.Element> _elementsToShieldFrom= new List<Elements.Element>();
    [SerializeField]private List<ElementalSpell> _elementalSpellsToShieldFrom = new List<ElementalSpell>();
    public UnityEvent<ElementalShield> OnShieldDisabled;

    public void SetUp(List<ElementalSpell> elementalSpells)
    {
        _elementsToShieldFrom = elementalSpells.Select(x=>x.Element).ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ElementalAffliction tmp= collision.attachedRigidbody.GetComponentInParent<ElementalAffliction>();
        if(tmp!=null)
        {
            tmp.AddElementsFromShield(this);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ElementalAffliction tmp = collision.attachedRigidbody.GetComponentInParent<ElementalAffliction>();
        if (tmp != null)
        {
            tmp.RemoveElementsFromShield(this);
        }
    }
    private void OnDisable()
    {
        OnShieldDisabled?.Invoke(this);
    }

}
