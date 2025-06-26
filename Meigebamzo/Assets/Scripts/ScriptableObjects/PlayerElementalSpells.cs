using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerElementalSpells")]
public class PlayerElementalSpells : ScriptableObject
{
    public ElementalSpell BasicElement => _basicElement;
    public Elements.ElementAttackType AttackType=>_attackType;

    [SerializeField] ElementalSpell _basicElement;
    [SerializeField] Elements.ElementAttackType _attackType;


}