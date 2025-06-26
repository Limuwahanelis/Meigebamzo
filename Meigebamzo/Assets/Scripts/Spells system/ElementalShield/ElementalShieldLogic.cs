using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementalShieldLogic
{
    public List<ElementalSpell> ElementalSpellsToShieldFrom => _elementalSpellsToShieldFrom;
    private List<ElementalSpell> _elementalSpellsToShieldFrom = new List<ElementalSpell>();
    public Action<ElementalShieldLogic> OnShieldDisabled;

    public void SetUp(List<ElementalSpell> elementalSpellsToShieldFrom)
    {
        _elementalSpellsToShieldFrom = elementalSpellsToShieldFrom;
    }
    public void DisableShield()
    {
        OnShieldDisabled?.Invoke(this);
    }
}
