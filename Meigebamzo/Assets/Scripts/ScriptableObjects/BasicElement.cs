using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BasicElement")]
public class BasicElement : ScriptableObject
{
    public List<Elements.Element> NegatingElements => _negatingElements;
    public Color AssociatedColor => _associatedColor;
    public Elements.Element Element => _element;
    public Sprite Sprite => _sprite;


    [SerializeField] Elements.Element _element;
    [SerializeField] Sprite _sprite;
    [SerializeField] Color _associatedColor;
    [SerializeField] List<Elements.Element> _negatingElements;

}