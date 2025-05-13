using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [SerializeField] ParticleSystem _thunderParticlesPrefab;
    [SerializeField] int _count;
    [SerializeField] float _startLifetime;
    [SerializeField] float _length;
    [SerializeField] List<ParticleSystem> _paritcles= new List<ParticleSystem>();
    private List<float> angles = new List<float>() {0,0 };
    private bool _canAttackElectricity = true;
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
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDied;
        Logger.Log("ADDed");
    }
    public void RemoveEnemyFromThunderAttack(GameObject enemy)
    {
        _enemiesInRange.Remove(enemy.GetComponent<Enemy>());
        enemy.GetComponent<HealthSystem>().OnDeath -= OnEnemyDied;
        Logger.Log("Removed");
    }
    private void OnEnemyDied(IDamagable damagable)
    {
        Enemy enemy = _enemiesInRange.Find(x => (IDamagable)x.GetComponent<HealthSystem>() == damagable);
        enemy.GetComponent<HealthSystem>().OnDeath -= OnEnemyDied;
        _enemiesInRange.Remove(_enemiesInRange.Find(x => (IDamagable)x.GetComponent<HealthSystem>() == damagable));
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
      
       
        Vector2 direction =RaycastFromCamera2D.MouseInWorldPos - _mainBody.position;
        direction.Normalize();
        ParticleSystem.EmitParams parameters = new ParticleSystem.EmitParams(); 
        float angle = Random.Range(-_spread, _spread);
        float spread = _spread;
        Enemy enemy = null;
        foreach (ParticleSystem p in _paritcles)
        {
            p.transform.transform.position = _mainBody.position;
        }

        if (_enemiesInRange.Count == 0)
        {
            parameters = SetUpThunderParticleParams(direction, 1);
        }
        else
        {
            spread = _spread / 2;
            enemy = GetClosesEnemyForThunder();
            direction = (enemy.EnemyRB.position - new Vector2(_mainBody.position.x, _mainBody.position.y)).normalized;
            parameters = SetUpThunderParticleParams(direction, Vector2.Distance(enemy.EnemyRB.transform.position, _mainBody.position) / 2);

        }
        int i = 0;
        foreach (ParticleSystem p in _paritcles)
        {
            p.transform.transform.up = direction;
           
            if (_canAttackElectricity)
            {
                angles[i] = angle;
                angle = Random.Range(-spread, spread);
                p.transform.transform.Rotate(transform.forward, angle);
                p.Emit(parameters, 1);
                if(enemy != null)
                {
                    enemy.GetComponent<HealthSystem>().TakeDamage(new DamageInfo(10, _mainBody.transform.position, Elements.Element.ELECTRICITY));
                }
            }
            else
            {
                p.transform.transform.Rotate(transform.forward, angles[i]);
            }
            i++;
            
        }
        StartCoroutine(ElectricityAttackCooldown(_thunderParticlesPrefab.main.startLifetime.constant-0.02f));
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
    private ParticleSystem.EmitParams SetUpThunderParticleParams(Vector2 direction,float length)
    {
        
        ParticleSystem.EmitParams parameters = new ParticleSystem.EmitParams();
        int spriteCount = _thunderParticlesPrefab.textureSheetAnimation.spriteCount;
        Vector3 size = new Vector3(_thunderParticlesPrefab.main.startSizeX.constant, length, _thunderParticlesPrefab.main.startSizeZ.constant);
        _thunderParticlesPrefab.transform.up = direction;
        parameters.startSize3D = size;
        return parameters;
    }
    IEnumerator ElectricityAttackCooldown(float cooldown)
    {
        if (!_canAttackElectricity) yield break;
        _canAttackElectricity = false;
        yield return new WaitForSeconds(cooldown);
        _canAttackElectricity = true;
    }
}
