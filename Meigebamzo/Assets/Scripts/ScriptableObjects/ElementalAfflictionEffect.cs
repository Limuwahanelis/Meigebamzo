using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ElementalAfflictionEffect")]
public class ElementalAfflictionEffect : ScriptableObject
{

    public int Damage => _damage;
    public float Duration => _duration;
    public float DamageCooldown => _damageCooldown;
    public BasicElement Element => _element;

    [SerializeField] float _duration;
    [SerializeField] int _damage;
    [SerializeField] float _damageCooldown;
    [SerializeField] BasicElement _element;
}