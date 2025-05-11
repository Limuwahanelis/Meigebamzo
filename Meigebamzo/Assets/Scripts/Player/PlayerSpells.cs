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
    [SerializeField] Transform _thunderAttackDetection;
    List<Enemy> _enemiesInRange = new List<Enemy>();
    private void Awake()
    {
        
    }
    private void Update()
    {
        _thunderAttackDetection.up = (RaycastFromCamera2D.MouseInWorldPos-_mainBody.position ).normalized;

    }
    public void SetEnemyForThunderAttack(GameObject enemy)
    {
        _enemiesInRange.Add(enemy.GetComponent<Enemy>());
        Logger.Log("ADDed");
    }
    public void RemoveEnemyFromThunderAttack(GameObject enemy)
    {
        _enemiesInRange.Remove(enemy.GetComponent<Enemy>());
        Logger.Log("Removed");
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
        if (_enemiesInRange.Count == 0) return;
        angle = 0;
        Enemy enemy = GetClosesEnemyForThunder();
        direction= (enemy.EnemyRB.position -new Vector2 (_mainBody.position.x, _mainBody.position.y)).normalized;
        sprite.transform.up = direction;
    }
    private Enemy GetClosesEnemyForThunder()
    {
        Enemy closestEnemy = _enemiesInRange[0];
        float closestDistance = Vector2.Distance(closestEnemy.EnemyRB.position, _mainBody.position);
        float dist = 0;
        foreach(Enemy enemy in _enemiesInRange)
        {
            dist = Vector2.Distance(enemy.EnemyRB.position, _mainBody.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
