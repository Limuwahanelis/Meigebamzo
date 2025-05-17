using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerElementalSpells")]
public class PlayerElementalSpells : ScriptableObject
{
    public Color AssociatedColor => _associatedColor;
    public Elements.Element Element =>_element;
    public Elements.ElementAttackType AttackType=>_attackType;

    public Sprite Sprite => _sprite;

    [SerializeField] Elements.Element _element;
    [SerializeField] Elements.ElementAttackType _attackType;
    [SerializeField] Sprite _sprite;
    [SerializeField] Color _associatedColor;
}