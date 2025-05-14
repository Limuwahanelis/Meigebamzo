using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAttack : ContinousAttack
{
    private Transform _mainBody;
    private float _spread;
    List<ParticleSystem> _particles = new List<ParticleSystem>();
    private List<float> _angles = new List<float>() { 0, 0 };
    PlayerSpells _playerSpells;
    ParticleSystem _thunderParticlesPrefab;

    bool _canAttackElectricity=true;
    public ElectricityAttack(Transform mainBody, float spread, List<ParticleSystem> particles, List<IDamagable> damageablesInRange, List<float> angles, PlayerSpells playerSpells, ParticleSystem thunderParticlesPrefab)
    {
        _mainBody = mainBody;
        _spread = spread;
        _particles = particles;
        _damageablesInRange = damageablesInRange;
        _angles = angles;
        _playerSpells = playerSpells;
        _thunderParticlesPrefab = thunderParticlesPrefab;
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
            parameters = SetUpThunderParticleParams(direction, 1);
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
                    damageable.TakeDamage(new DamageInfo(10, _mainBody.transform.position, Elements.Element.ELECTRICITY));
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
       
    }

    public override void StartAttack()
    {
        
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
