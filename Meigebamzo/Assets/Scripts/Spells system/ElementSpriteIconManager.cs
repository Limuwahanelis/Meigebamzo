using System.Collections.Generic;
using UnityEngine;

public class ElementSpriteIconManager : MonoBehaviour
{
    public List<Sprite> ElementIconSprites => _elementIconSprites;
    [SerializeField] List<Sprite> _elementIconSprites = new List<Sprite>();
}
