using System.Collections.Generic;
using UnityEngine;

public interface IBasicElement
{
    public List<Elements.Element> NegatingElements { get; }
    public Color AssociatedColor { get; }
    public Elements.Element Element { get; }
    public Sprite Sprite { get; }
}