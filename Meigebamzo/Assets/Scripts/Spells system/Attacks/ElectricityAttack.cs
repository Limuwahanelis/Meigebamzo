using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAttack : ContinousAttack
{
    private AllEnemiesList _enemiesList;
    private List<IDamagable> _electricityTargets=new List<IDamagable>();
    List<ParticleListWrapper> _allparticles = new List<ParticleListWrapper>();

    List<ParticleSystem> _particles = new List<ParticleSystem>();
    PlayerSpells _playerSpells;
    ParticleSystem _thunderParticlesPrefab;
    BoxCollider2D _electricityTrigger;
    Transform _mainBody;
    List<float> _angles = new List<float>() { 0, 0 };
    //Elements.Element _damageElement = Elements.Element.ELECTRICITY;
    ParticleSystem.MinMaxGradient _particlesStandardColor;
    AudioEvent _audioEvent;
    AudioSource _audioSource;

    ParticleSystem.MinMaxGradient _particlesNewColor;
    float _triggerStartingSize;
    float _spread;
    float _triggerStartingOffset;
    float _waterDMGNult=1.2f;
    int _baseAttack=10;
    int _additionalAttackPerElement = 3;
    int _numberOfAdditionalElectricityElements = 0;

    bool _canAttackElectricity=true;
    public ElectricityAttack(Transform mainBody, float spread, List<ParticleSystem> particles, List<IDamagable> damageablesInRange, List<float> angles, 
        PlayerSpells playerSpells, ParticleSystem thunderParticlesPrefab,BoxCollider2D electricityTrigger,
        List<ParticleListWrapper> allparticles,AudioEvent audioEvent,AudioSource audioSource)
    {
        _allparticles = allparticles;
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
        _audioEvent = audioEvent;
        _audioSource = audioSource;
    }
    public override void SetSpells(List<PlayerElementalSpells> spells)
    {
        base.SetSpells(spells);
        _basicElement = _spells.Find(x => x.BasicElement.Element == Elements.Element.ELECTRICITY).BasicElement;
        PlayerElementalSpells fireSpell = _spells.Find(x => x.BasicElement.Element == Elements.Element.FIRE);
        if (fireSpell != null) _basicElement = fireSpell.BasicElement;
    }
    public override void Attack()
    {
        _electricityTargets.Clear();
        Vector2 direction = RaycastFromCamera2D.MouseInWorldPos - _mainBody.position;
        ParticleSystem.EmitParams parameters = new ParticleSystem.EmitParams();
        float angle = UnityEngine.Random.Range(-_spread, _spread);
        float spread = _spread;
        float waterDamageMult = 1;
       
        foreach (ParticleSystem p in _allparticles[0].particles)
        {
            p.transform.transform.position = _mainBody.position;
        }

        if (_damageablesInRange.Count == 0)
        {
            int k = 0;
            parameters = SetUpThunderParticleParams(direction, 1 + 0.5f * _numberOfAdditionalElectricityElements / 2 - 0.125f);
             foreach(ParticleSystem p in _allparticles[0].particles)
            {
                if (_canAttackElectricity)
                {
                    _angles[k] = angle;
                    angle = UnityEngine.Random.Range(-spread, spread);

                    p.Emit(parameters, 1);
                }
                else
                {
                    p.transform.transform.position = _mainBody.position;
                    p.transform.transform.up = direction;
                    p.transform.transform.Rotate(_playerSpells.transform.forward, _angles[k]);
                }
                k++;
            }
        }
        else
        {
            Transform currMiddlePoint = _mainBody;
            SetElectricitytargets(GetClosestDamageableForThunder());


            direction.Normalize();
             angle = UnityEngine.Random.Range(-_spread, _spread);
             spread = _spread;
            IDamagable damageable = null;

            for (int i = 0; i < _electricityTargets.Count; i++)
            {
                if (_canAttackElectricity)
                {
                    direction = (_electricityTargets[i].MainBody.position - currMiddlePoint.position).normalized;

                    int j = 0;
                    parameters = SetUpThunderParticleParams(direction, Vector2.Distance(_electricityTargets[i].MainBody.position, currMiddlePoint.position) / 2);
                    foreach (ParticleSystem p in _allparticles[i].particles)
                    {
                        p.transform.transform.position = currMiddlePoint.position;
                        p.transform.transform.up = direction;
                        _angles[j] = angle;
                        angle = UnityEngine.Random.Range(-spread, spread);
                        p.transform.transform.Rotate(currMiddlePoint.forward, angle);
                        p.Emit(parameters, 1);
                        damageable = _electricityTargets[i];
                        if (damageable != null)
                        {
                            if (damageable.ElementalAffliction.ElementAffectedBy == Elements.Element.WATER) waterDamageMult = _waterDMGNult;
                            damageable.TakeDamage(new DamageInfo((int)((_baseAttack + _additionalAttackPerElement * _numberOfAdditionalElectricityElements)*waterDamageMult), _mainBody.transform.position, _basicElement));
                        }
                    }
                    currMiddlePoint = _electricityTargets[i].MainBody;
                }
            }
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
        _electricityTrigger.enabled = false;
        _electricityTargets.Clear();
        _particlesNewColor = _particlesStandardColor;

        ParticleSystem.MainModule mainMod = _thunderParticlesPrefab.main;
        mainMod.startColor = _particlesStandardColor;

        _audioEvent.Pause(_audioSource);
    }

    public override void StartAttack()
    {
        _audioEvent.Play(_audioSource);
        _numberOfAdditionalElectricityElements = _spells.FindAll(x => x.BasicElement.Element == Elements.Element.ELECTRICITY).Count-1;
        Vector2 size = _electricityTrigger.size;
        Vector2 offset = _electricityTrigger.offset;
        size.y += 0.5f * _numberOfAdditionalElectricityElements;
        offset.y+=( 0.5f * _numberOfAdditionalElectricityElements)/2;
        _electricityTrigger.size = size;
        _electricityTrigger.offset = offset;
        _electricityTrigger.enabled = true;
        Elements.Element damageElement = _basicElement.Element;

        _particlesNewColor = _particlesStandardColor;
        
        ParticleSystem.MainModule mainMod = _thunderParticlesPrefab.main;
        mainMod.startColor = _particlesStandardColor;

        if (_spells.Find(x => x.BasicElement.Element == Elements.Element.FIRE))
        {
            damageElement = Elements.Element.FIRE;
            
            
        }
        _particlesNewColor.color = _spells.Find(x => x.BasicElement.Element == damageElement).BasicElement.AssociatedColor;
        mainMod.startColor = _particlesNewColor;
        foreach (ParticleSystem p in _particles)
        {
            mainMod = p.main;
            mainMod.startColor = _particlesNewColor;
            
        }

        foreach(ParticleListWrapper pList in _allparticles)
        {
            foreach(ParticleSystem p in pList.particles)
            {
                mainMod = p.main;
                mainMod.startColor = _particlesNewColor;
            }
        }

    }
    private void SetElectricitytargets(IDamagable firstTarget)
    {
        _electricityTargets.Add(firstTarget);
        IDamagable currMiddlePoint = _electricityTargets[0];
        IDamagable currenClosestTran = currMiddlePoint;
        Vector2 middleToMouseVector;
        float maxDotProduct = 0;
        for (int i=0;i<3;i++)
        {
            List<IDamagable> potentialtargets = AllEnemiesList.AllEnemiesTransform.FindAll(x => Vector2.Distance(currMiddlePoint.MainBody.position, x.MainBody.position) < 5f && x!= currMiddlePoint&&!_electricityTargets.Contains(x));
            if (potentialtargets.Count == 0) return;
            middleToMouseVector = (currMiddlePoint.MainBody.position - RaycastFromCamera2D.MouseInWorldPos).normalized;
            currenClosestTran = potentialtargets[0];
            maxDotProduct = Vector2.Dot((RaycastFromCamera2D.MouseInWorldPos - currenClosestTran.MainBody.position).normalized, middleToMouseVector);
            for (int j=1;j<potentialtargets.Count;j++)
            {
                Vector2 middleToPotentialVector = (currMiddlePoint.MainBody.position - potentialtargets[j].MainBody.position).normalized;
                if (Vector2.Dot(middleToPotentialVector, middleToMouseVector) >maxDotProduct)
                {
                    currenClosestTran = potentialtargets[j];
                    maxDotProduct = Vector2.Dot(currMiddlePoint.MainBody.position - potentialtargets[j].MainBody.position, middleToMouseVector);
                }
            }
            _electricityTargets.Add(currenClosestTran);
            currMiddlePoint = currenClosestTran;
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
        _electricityTargets.Clear();
    }
}
