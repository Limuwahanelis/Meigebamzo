using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerElementalSpells")]
public class PlayerElementalSpells : ScriptableObject
{
    public BasicElement BasicElement => _basicElement;
    public Elements.ElementAttackType AttackType=>_attackType;

    [SerializeField] BasicElement _basicElement;
    [SerializeField] Elements.ElementAttackType _attackType;


}