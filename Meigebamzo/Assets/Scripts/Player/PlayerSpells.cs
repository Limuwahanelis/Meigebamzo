using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] List<PlayerElementalSpells> _availableElementalSpells;
    [SerializeField] int _maxNumberOfSelectedElements;
    [SerializeField] Transform _mainBody;
    private List<PlayerElementalSpells> _selectedElements = new List<PlayerElementalSpells>();

    [Header("Electricity")]
    [Tooltip("Spread in degrees"),SerializeField] float _spread=2f;
    [SerializeField] SpritePool _thunderSpritePool;
    [SerializeField] float _thunderDuration=0.1f;
    [SerializeField] float _thunderCooldown = 0.5f;
    private void Awake()
    {
        
    }
    public void AddElement(Elements.Element element)
    {

        if(_selectedElements.Count< _maxNumberOfSelectedElements)
        {
            Logger.Log(element.ToString());
            _selectedElements.Add(_availableElementalSpells.Find(x=>x.Element==element));
        }
    }

    public void ElectricityAttack()
    {
        ElementSprite sprite=  _thunderSpritePool.GetItem();
        sprite.transform.position = _mainBody.position;
        Vector2 direction =RaycastFromCamera2D.MouseInWorldPos - _mainBody.position;
        direction.Normalize();
        sprite.SetUp(_thunderDuration);
        sprite.transform.up = direction;
        float angle = Random.Range(-_spread, _spread);
        sprite.transform.Rotate(transform.forward, angle);
    }
}
