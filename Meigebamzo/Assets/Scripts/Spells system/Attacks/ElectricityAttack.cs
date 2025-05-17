using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAttack : ContinousAttack
{
    List<ParticleSystem> _particles = new List<ParticleSystem>();
    PlayerSpells _playerSpells;
    ParticleSystem _thunderParticlesPrefab;
    BoxCollider2D _electricityTrigger;
    Transform _mainBody;
    List<float> _angles = new List<float>() { 0, 0 };
    Elements.Element _damageElement = Elements.Element.ELECTRICITY;
    ParticleSystem.MinMaxGradient _particlesStandardColor;

    ParticleSystem.MinMaxGradient _particlesNewColor;
    float _triggerStartingSize;
    float _spread;
    float _triggerStartingOffset;

    int _additionalElectricityElements = 0;

    bool _canAttackElectricity=true;
    public ElectricityAttack(Transform mainBody, float spread, List<ParticleSystem> particles, List<IDamagable> damageablesInRange, List<float> angles, PlayerSpells playerSpells, ParticleSystem thunderParticlesPrefab,BoxCollider2D electricityTrigger)
    {

        _mainBody = mainBody;
        _spread = spread;
        _particles = particles;
        _damageablesInRange = damageablesInRange;
        _angles = angles;
        _playerSpells = playerSpells;
        _thunderParticlesPrefab = thunderParticlesPrefab;
        _electricityTrigger = electricityTrigger;
        _triggerStartingSize = electricityTrigger.size.y;
        _triggerStartingOffset = electricityTrigger.offset.y;
        _particlesStandardColor = thunderParticlesPrefab.main.startColor;
        _particlesNewColor = _particlesStandardColor;
    }

    public override void Attack()
    {
        Vector2 direction = RaycastFromCamera2D.MouseInWorldPos - _mainBody.position;
        direction.Normalize();
        ParticleSystem.EmitParams parameters = new ParticleSystem.EmitParams();
        float angle = UnityEngine.Random.Range(-_spread, _spread);
        float spread = _spread;
        IDamagable damageable = null;
        foreach (ParticleSystem p in _particles)
        { 
            p.transform.transform.position = _mainBody.position;
        }

        if (_damageablesInRange.Count == 0)
        {
            parameters = SetUpThunderParticleParams(direction, 1+0.5f*_additionalElectricityElements/2 -0.125f);
        }
        else
        {
            spread = _spread / 2;
            damageable = GetClosestDamageableForThunder();
            direction = ((Vector2)damageable.Transform.position - new Vector2(_mainBody.position.x, _mainBody.position.y)).normalized;
            parameters = SetUpThunderParticleParams(direction, Vector2.Distance(damageable.Transform.position, _mainBody.position) / 2);

        }
        int i = 0;
        foreach (ParticleSystem p in _particles)
        {
            p.transform.transform.up = direction;

            if (_canAttackElectricity)
            {
                _angles[i] = angle;
                angle = UnityEngine.Random.Range(-spread, spread);
                p.transform.transform.Rotate(_playerSpells.transform.forward, angle);
                p.Emit(parameters, 1);
                if (damageable != null)
                {
                    damageable.TakeDamage(new DamageInfo(10+3*_additionalElectricityElements, _mainBody.transform.position, _damageElement));
                }
            }
            else
            {
                p.transform.transform.Rotate(_playerSpells.transform.forward, _angles[i]);
            }
            i++;

        }
        _playerSpells.StartCoroutine(ElectricityAttackCooldown(_thunderParticlesPrefab.main.startLifetime.constant - 0.02f));
    }

    public override void EndAttack()
    {
        _spells.Clear();
        Vector2 size = _electricityTrigger.size;
        Vector2 offset = _electricityTrigger.offset;
        size.y = _triggerStartingSize;
        offset.y = _triggerStartingOffset;
        _electricityTrigger.size = size;
        _electricityTrigger.offset = offset;

        _particlesNewColor = _particlesStandardColor;

        ParticleSystem.MainModule mainMod = _thunderParticlesPrefab.main;
        mainMod.startColor = _particlesStandardColor;

    }

    public override void StartAttack()
    {
        _additionalElectricityElements = _spells.FindAll(x => x.Element == Elements.Element.ELECTRICITY).Count-1;
        Vector2 size = _electricityTrigger.size;
        Vector2 offset = _electricityTrigger.offset;
        size.y += 0.5f * _additionalElectricityElements;
        offset.y+=( 0.5f * _additionalElectricityElements)/2;
        _electricityTrigger.size = size;
        _electricityTrigger.offset = offset;

        _damageElement = Elements.Element.ELECTRICITY;

        _particlesNewColor = _particlesStandardColor;
        
        ParticleSystem.MainModule mainMod = _thunderParticlesPrefab.main;
        mainMod.startColor = _particlesStandardColor;

        if (_spells.Find(x => x.Element == Elements.Element.FIRE))
        {
            _damageElement = Elements.Element.FIRE;
            
            
        }
        _particlesNewColor.color = _spells.Find(x => x.Element == _damageElement).AssociatedColor;
        mainMod.startColor = _particlesNewColor;
        foreach (ParticleSystem p in _particles)
        {
            mainMod = p.main;
            mainMod.startColor = _particlesNewColor;
            
        }

    }
    private IDamagable GetClosestDamageableForThunder()
    {
        IDamagable closestDamageable = _damageablesInRange[0];
        float closestDistance = Vector2.Distance(closestDamageable.Transform.position, _mainBody.position);
        float dist = 0;
        foreach (IDamagable damagable in _damageablesInRange)
        {
            dist = Vector2.Distance(damagable.Transform.position, _mainBody.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestDamageable = damagable;
            }
        }
        return closestDamageable;
    }
    private ParticleSystem.EmitParams SetUpThunderParticleParams(Vector2 direction, float length)
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
