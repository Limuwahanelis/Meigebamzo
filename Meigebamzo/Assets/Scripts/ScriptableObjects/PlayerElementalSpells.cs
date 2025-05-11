using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerElementalSpells")]
public class PlayerElementalSpells : ScriptableObject
{
    public Elements.Element Element =>_element;
    public Elements.ElementAttackType AttackType=>_attackType;

    [SerializeField] Elements.Element _element;
    [SerializeField] Elements.ElementAttackType _attackType;

}