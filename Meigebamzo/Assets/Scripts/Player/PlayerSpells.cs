using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] List<PlayerElementalSpells> _availableElementalSpells;
    [SerializeField] int _maxNumberOfSelectedElements;
    private List<PlayerElementalSpells> _selectedElements = new List<PlayerElementalSpells>();
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
}
