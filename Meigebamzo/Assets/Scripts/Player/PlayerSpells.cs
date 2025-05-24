using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRenderer
{
    public PlayerElementalSpells Spell;
    public SpriteRenderer SpriteRenderer;
    public IconSlot IconSlot;
    public int Index;
}
public class IconSlot
{
    public PlayerElementalSpells Spell;
    public IconRenderer AssignedIconRenderer;
    public Transform Slot;
    public int Index;
}
[System.Serializable]
public class ParticleListWrapper
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();
}
public class PlayerSpells : MonoBehaviour
{
    [SerializeField] List<PlayerElementalSpells> _availableElementalSpells;
    [SerializeField] int _maxNumberOfSelectedElements;
    [SerializeField] Transform _mainBody;
    [SerializeField] List<SpriteRenderer> _spellSlotsRenderes = new List<SpriteRenderer>();
    [SerializeField] List<Transform> _spellIconsSlots = new List<Transform>();
    [SerializeField] Transform _spellIconSpawn;
    [SerializeField] float _spellIconMoveSpeed;
    [SerializeField] ElementSpellsIconDestoryer _spellIconDestoryer;
    [SerializeField] PlayerElementalSpells _physicalSpell;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource _audioSourceNotLooped;
    private List<PlayerElementalSpells> _selectedElements = new List<PlayerElementalSpells>();

    [Header("Electricity")]
    [Tooltip("Spread in degrees"),SerializeField] float _spread=2f;
    [SerializeField] AudioEvent _electricityAudioEvent;
    [SerializeField] SpritePool _thunderSpritePool;
    [SerializeField] float _thunderDuration=0.1f;
    [SerializeField] float _thunderCooldown = 0.5f;
    [SerializeField] Transform _thunderAttackDetection;
    [SerializeField] ParticleSystem _thunderParticlesPrefab;
    [SerializeField] int _count;
    [SerializeField] float _startLifetime;
    [SerializeField] float _length;
    [SerializeField] BoxCollider2D _electricityTrigger;
    [SerializeField] List<ParticleSystem> _paritcles= new List<ParticleSystem>();
    [SerializeField] List<ParticleListWrapper> _allparticles= new List<ParticleListWrapper>();
    [Header("Fire")]
    [SerializeField] AudioEvent _fireAudioEvent;
    [SerializeField] float _fireRange;
    [SerializeField] float _fireAngle;
    [SerializeField] Transform _fireEndTran;
    [SerializeField] Transform _testTran;
    [SerializeField] ParticleSystem _fireParticleSystem;
    [SerializeField] PolygonCollider2D _fireTrigger;
    [SerializeField] int _fireAttackDamage;
    [SerializeField] float _fireAttackCooldown;
    [Header("Water")]
    [SerializeField] float _waterRange;
    [SerializeField] float _waterAngel;
    [SerializeField] Transform _waterEndTran;
    [SerializeField] ParticleSystem _waterParticleSystem;
    [SerializeField] PolygonCollider2D _waterTrigger;
    [SerializeField] int _waterAttackDamage;
    [SerializeField] float _waterAttackCooldown;
    [SerializeField] AudioEvent _waterAttackAudioevent;
    [Header("Wind")]
    [SerializeField] AudioEvent _airAttackAudioEvent;
    [SerializeField] List<Transform> _windPushTrans= new List<Transform>();
    [SerializeField] List<WindPush> _windPushes= new List<WindPush>();

    private int _corIndex = 0;
    private List<float> angles = new List<float>() {0,0 };
    private bool _canAttackElectricity = true;
    private ContinousAttack _cutrrentContinousAttack;
    private List<Vector2> _fireTriangle;
    private List<SpellCoroutineWrapper> _movingSpellIconsCors= new List<SpellCoroutineWrapper>();
    private List<IconRenderer> _iconsRenderers = new List<IconRenderer>();
    private List<IconSlot> _iconsSlots = new List<IconSlot>();
    bool _removeOldCorutines = true;
    List<IDamagable> _damageablesInRange = new List<IDamagable>();
    Dictionary<Elements.Element, ContinousAttack> _continousAttacks= new Dictionary<Elements.Element, ContinousAttack>();
    private void Awake()
    {
        for(int i=0;i<4;i++)
        {
            _iconsRenderers.Add(new IconRenderer() { SpriteRenderer = _spellSlotsRenderes[i],Index=i,Spell=_physicalSpell });
        }
        for (int i = 0; i < 4; i++)
        {
            _iconsSlots.Add(new IconSlot() { Slot = _spellIconsSlots[i],Index=i,Spell= _physicalSpell });
        }
        foreach (SpriteRenderer spriteRenderer in _spellSlotsRenderes)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
        _continousAttacks.Add(Elements.Element.FIRE, new FireAttack(this,_fireParticleSystem, _damageablesInRange, _fireTrigger, 
            _mainBody, _fireRange, _fireAngle,_fireAttackDamage,_fireAttackCooldown, _fireAudioEvent,_audioSource));
        _continousAttacks.Add(Elements.Element.ELECTRICITY, new ElectricityAttack(_mainBody, _spread, _paritcles, _damageablesInRange, angles, this, _thunderParticlesPrefab,
            _electricityTrigger,_allparticles, _electricityAudioEvent,_audioSource));
        _continousAttacks.Add(Elements.Element.WATER, new WaterAttack(this, _waterParticleSystem, _damageablesInRange,_waterTrigger,
            _mainBody, _waterRange, _waterAngel, 0, _waterAttackCooldown, _waterAttackAudioevent,_audioSource));
    }
    private void Update()
    {
        _thunderAttackDetection.up = (RaycastFromCamera2D.MouseInWorldPos-_mainBody.position ).normalized;

    }
    public void SetEnemyForAttack(GameObject enemy)
    {
        _damageablesInRange.Add(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<IDamagable>().OnDeath += OnEnemyDied;
        Logger.Log("ADDed");
    }
    public void RemoveEnemyFromAttack(GameObject enemy)
    {
        _damageablesInRange.Remove(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<IDamagable>().OnDeath -= OnEnemyDied;
        Logger.Log("Removed");
    }
    private void OnEnemyDied(IDamagable damagable)
    {
        damagable.OnDeath -= OnEnemyDied;
        _damageablesInRange.Remove(damagable);
    }
    private void RemoveEnemiesFromRange()
    {
        _damageablesInRange.Clear();
    }
    public void AddElement(Elements.Element element)
    {
        Logger.Log(_movingSpellIconsCors.Count);
        if(_selectedElements.Count< _maxNumberOfSelectedElements)
        {
            //Logger.Log(element.ToString());
            bool destroyIcons = false;
            PlayerElementalSpells spell = _availableElementalSpells.Find(x => x.Element == element);
            _selectedElements.Add(spell);

            if(_selectedElements.Count>1)
            {
                IconRenderer negatingIcon = _iconsRenderers.Find(x => x.Spell.NegatingElements.Contains(element));
                if (negatingIcon != null)
                {
                    IconSlot negatingSlot = negatingIcon.IconSlot;
                    SpellCoroutineWrapper cor = _movingSpellIconsCors.Find(x => x.Icon.Spell.NegatingElements.Contains( element));
                    if(cor!=null )
                    {
                        if( cor.Cor != null) StopCoroutine(cor.Cor);
                        _movingSpellIconsCors.Remove(cor);
                    }
                    _selectedElements.Remove(spell);
                    _selectedElements.Remove(negatingSlot.Spell);
                    negatingSlot.AssignedIconRenderer.SpriteRenderer.gameObject.SetActive(false);
                    negatingSlot.AssignedIconRenderer.Spell = _physicalSpell;
                    negatingSlot.AssignedIconRenderer.IconSlot = null;
                    negatingSlot.AssignedIconRenderer = null;
                    negatingSlot.Spell = _physicalSpell;
                    _selectedElements.Remove(negatingSlot.Spell);
                    AdjustIcons();
                    return;
                }


            }
            IconRenderer iconRenderer = _iconsRenderers.Find(x=>x.IconSlot == null);
            iconRenderer.SpriteRenderer.sprite = spell.Sprite;
            iconRenderer.IconSlot = _iconsSlots.Find(x=>x.AssignedIconRenderer == null);
            iconRenderer.IconSlot.Spell = spell;
            iconRenderer.SpriteRenderer.transform.position = _spellIconSpawn.position;
            iconRenderer.Spell = spell;
            iconRenderer.IconSlot.AssignedIconRenderer = iconRenderer;
            iconRenderer.SpriteRenderer.gameObject.SetActive(true);
            SpellCoroutineWrapper newCor = new SpellCoroutineWrapper();
            newCor.Cor = StartCoroutine(MoveSpellIconToSlot(iconRenderer, _corIndex, 0, iconRenderer.Index, false));
            newCor.index = _corIndex;
            newCor.Icon = iconRenderer;
            _movingSpellIconsCors.Add(newCor);
            _corIndex++;

            //int slotIndex = _selectedElements.Count - 1;
            //int iconRendererIndex = _iconsRenderers.FindIndex(x=>x.IconSlot==null);
            //int negatingIconIndex=iconRendererIndex;
            //int negatingIconSlot;
            //_spellSlotsRenderes[iconRendererIndex].sprite = spell.Sprite;
            //_spellSlotsRenderes[iconRendererIndex].transform.position = _spellIconSpawn.position;

            //IconSlot iconSlot = _iconsSlots.Find(x => x.AssignedIconRenderer == null);
            //iconSlot.Spell = spell;
            //iconSlot.AssignedIconRenderer = _iconsRenderers[iconRendererIndex];
            //_iconsRenderers[iconRendererIndex].IconSlot = iconSlot;
            //_spellSlotsRenderes[iconRendererIndex].gameObject.SetActive(true);
            //if (_selectedElements.Count > 1)
            //{
            //    IconSlot negatingSlot = _iconsSlots.Find(x => x.Spell.NegatingElements.Contains(element));
            //    if (negatingSlot != null)
            //    {
            //        negatingIconSlot = negatingSlot.Index;


            //        if (negatingIconSlot != -1)
            //        {

            //            destroyIcons = true;
            //            negatingIconIndex = _iconsSlots[negatingIconSlot].AssignedIconRenderer.Index;
            //            _iconsRenderers[negatingIconIndex].SpriteRenderer.sortingOrder = 2;
            //            _selectedElements.RemoveAt(_selectedElements.Count - 1);
            //            _selectedElements.RemoveAt(negatingIconSlot);


            //            _iconsRenderers[iconRendererIndex].IconSlot = _iconsSlots[negatingIconSlot + 1];
            //            _iconsRenderers[iconRendererIndex].SpriteRenderer.sortingOrder = 2;
            //        }
            //    }

            //}
            //CoroutineWrapper coroutineWrapper = new CoroutineWrapper();
            //coroutineWrapper.NegatingIcon = _iconsRenderers[negatingIconIndex];
            //coroutineWrapper.Icon = _iconsRenderers[iconRendererIndex];
            //coroutineWrapper.DestroyIcons = destroyIcons;
            //coroutineWrapper.Cor = StartCoroutine(MoveSpellIconToSlot(_iconsRenderers[iconRendererIndex].IconSlot.Slot, _iconsRenderers[iconRendererIndex].SpriteRenderer.transform, _corIndex, negatingIconIndex, iconRendererIndex, destroyIcons));
            //coroutineWrapper.index = _corIndex;
            //_corIndex++;
            //_movingSpellIconsCors.Add(coroutineWrapper);
        }
    }
    private void AdjustIcons()
    {

        List<IconRenderer> activeElementsIcon = _iconsRenderers.FindAll(x => x.IconSlot!=null);
        for(int i=0;i<activeElementsIcon.Count;i++)
        {
            activeElementsIcon[i].IconSlot.AssignedIconRenderer = null;
            activeElementsIcon[i].IconSlot = _iconsSlots.Find(x => x.AssignedIconRenderer == null);
            activeElementsIcon[i].IconSlot.AssignedIconRenderer = activeElementsIcon[i];
            activeElementsIcon[i].IconSlot.Spell = activeElementsIcon[i].Spell;
            if(_movingSpellIconsCors.Find(x=>x.Icon== activeElementsIcon[i])==null)
            {
                SpellCoroutineWrapper cor = new SpellCoroutineWrapper();
                cor.index = _corIndex;
                cor.Cor = StartCoroutine(MoveSpellIconToSlot(activeElementsIcon[i], _corIndex, 0, activeElementsIcon[i].Index, false));
                cor.Icon = activeElementsIcon[i];
                _corIndex++;

            }
        }
        //CoroutineWrapper wrapper;
        //for(int i = 0; i< activeElementsIcon.Count;i++)
        //{
        //    wrapper = _movingSpellIconsCors.Find(x => x.Icon == activeElementsIcon[i]);
        //    IconSlot oldSlot = wrapper.Icon.IconSlot;
        //    wrapper.Icon.IconSlot = _iconsSlots.Find(x => x.AssignedIconRenderer == null);
        //    wrapper.Icon.IconSlot.Spell = oldSlot.Spell;
        //    oldSlot.AssignedIconRenderer = null;
        //    oldSlot.Spell = _physicalSpell;
        //    wrapper.Icon.IconSlot.AssignedIconRenderer = wrapper.Icon;
        //    if (wrapper.Cor!=null) StopCoroutine(wrapper.Cor);

        //    _corIndex++;
        //    wrapper.Cor= StartCoroutine(MoveSpellIconToSlot(wrapper.Icon.IconSlot.Slot, wrapper.Icon.SpriteRenderer.transform, _corIndex, wrapper.NegatingIcon.Index, wrapper.IconIndex, wrapper.DestroyIcons));
        //    wrapper.index = _corIndex;
        //}
        //for(int i=0;i< activeElementsIcon.Count;i++)
        //{
        //    CoroutineWrapper coroutineWrapper = new CoroutineWrapper();
        //    iconIndex = _iconsRenderers.FindIndex(x => x.SpriteRenderer == activeElementsIcon[i]);
        //    _iconsRenderers[iconIndex].SlotTransform = _spellIconsSlots[i];
        //    if (i < activeElementsIcon.Count - 1)
        //    {
        //        negatingIconRendererIndex = _selectedElements.FindIndex(x => x.NegatingElements.Contains(_selectedElements[i].Element));
        //        destroyIcons = true;
        //    }
        //    coroutineWrapper.Cor = StartCoroutine(MoveSpellIconToSlot(_spellIconsSlots[i], activeElementsIcon[i].transform, _corIndex, negatingIconRendererIndex, iconIndex, destroyIcons));
        //    _movingSpellIconsCors.Add(coroutineWrapper);
        //    _corIndex++;
        //}
    }
    private Elements.Element DetermineAttack()
    {
        if(_selectedElements.Find(x=>x.Element==Elements.Element.ELECTRICITY)!=null) return Elements.Element.ELECTRICITY;
        else if(_selectedElements.Find(x=>x.Element==Elements.Element.FIRE) != null) return Elements.Element.FIRE;
        else if (_selectedElements.Find(x => x.Element == Elements.Element.WATER) != null) return Elements.Element.WATER;
        else if (_selectedElements.Find(x=>x.Element==Elements.Element.WIND) != null) return Elements.Element.WIND;
        return Elements.Element.PHYSICAL;
    }
    public bool StartAttack()
    {
        for(int i=0;i<_movingSpellIconsCors.Count;i++)
        {
            if (_movingSpellIconsCors[i].Cor != null) StopCoroutine(_movingSpellIconsCors[i].Cor);

        }
        _movingSpellIconsCors.Clear();
        for (int i=0;i<4;i++)
        {
            _iconsRenderers[i].IconSlot = null;
            _iconsRenderers[i].Spell = _physicalSpell;
            _iconsRenderers[i].SpriteRenderer.gameObject.SetActive(false);
            _iconsSlots[i].AssignedIconRenderer = null;
            _iconsSlots[i].Spell = _physicalSpell;
        }
        Elements.Element attackElement = DetermineAttack();
        if (_selectedElements.Count == 0) return false;
        if (attackElement == Elements.Element.PHYSICAL)
        {
            _selectedElements.Clear();
            foreach(SpriteRenderer spriteRenderer in _spellSlotsRenderes)
            {
                spriteRenderer.gameObject.SetActive(false);
            }
            return false;
        }
        foreach (SpriteRenderer spriteRenderer in _spellSlotsRenderes)
        {
            spriteRenderer.gameObject.SetActive(false);
        }

        if (_continousAttacks.TryGetValue(attackElement, out _))
        {
            _cutrrentContinousAttack = _continousAttacks[attackElement];

            _cutrrentContinousAttack.SetSpells(_selectedElements);
            _selectedElements.Clear();
            _cutrrentContinousAttack.StartAttack();
            return true;
        }

        if(attackElement==Elements.Element.WIND)
        {
            _airAttackAudioEvent.Play(_audioSourceNotLooped);
            int windForce = _selectedElements.FindAll(x => x.Element == Elements.Element.WIND).Count-1;

            _windPushes[windForce].transform.position= _windPushTrans[windForce].transform.position;
            _windPushes[windForce].transform.up = _electricityTrigger.transform.up;
            _windPushes[windForce].transform.Rotate(_windPushes[windForce].transform.forward, 45f);
            _windPushes[windForce].SetPushForce(windForce + 1);
            _windPushes[windForce].GetComponent<Animator>().SetInteger("PushType",windForce);
            _windPushes[windForce].GetComponent<Animator>().SetTrigger("Push");
            _windPushes[windForce].transform.SetParent(null);
            _selectedElements.Clear();
        }

        return false;
    }
    public void Attack()
    {
        _cutrrentContinousAttack.Attack();
    }
    public void EndAttack()
    {
        if (_cutrrentContinousAttack == null) return;
        _cutrrentContinousAttack.EndAttack();
        RemoveEnemiesFromRange();
        _cutrrentContinousAttack = null;
    }
  
    List<Vector2> GetTriangle()
    {
        _fireEndTran.position=_mainBody.transform.position;
        List<Vector2> toReturn = new List<Vector2>() {new Vector2(),new Vector2(),new Vector2() };
        Vector2 pointA = _mainBody.transform.position;
        Vector2 mouseDir= (RaycastFromCamera2D.MouseInWorldPos- _mainBody.transform.position).normalized;
        Vector2 fireForwardPoint=pointA+mouseDir*_fireRange;
        Vector2 fireForwardDir = (fireForwardPoint- (Vector2)_mainBody.transform.position);

        float mult=fireForwardPoint.magnitude;
        _fireEndTran.up = fireForwardDir.normalized;

        Quaternion rot= Quaternion.AngleAxis(_fireAngle / 2, Vector3.forward);
        Vector2 ABdir = rot * fireForwardDir;

        float aFunParam = ABdir.y / ABdir.x;

        rot = Quaternion.AngleAxis(-_fireAngle / 2, Vector3.forward);
        Vector2 ACdir = rot * fireForwardDir ;

        toReturn[0]=pointA+ABdir;
        toReturn[1] = pointA;
        toReturn[2] = pointA+ACdir;
        _fireEndTran.position = fireForwardPoint;
        

        return toReturn;
    }

    bool SameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
    {
        Vector3 cp1 = Vector3.Cross(b - a, p1 - a);
        Vector3 cp2 = Vector3.Cross(b - a, p2 - a);
        if (Vector3.Dot(cp1, cp2) >= 0) return true;
        else return false;
    }

    bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        if (SameSide(p, a, b, c) && SameSide(p, b, a, c)
            && SameSide(p, c, a, b)) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        List<Vector2> points = GetTriangle();

        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[0], points[2]);
        Gizmos.DrawSphere(points[1]+ (Vector2)(RaycastFromCamera2D.MouseInWorldPos - _mainBody.transform.position).normalized*_fireRange, 0.3f);
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawLine(_mainBody.position, _mainBody.position + (RaycastFromCamera2D.MouseInWorldPos - _mainBody.transform.position));
    }

    private IEnumerator MoveSpellIconToSlot(IconRenderer iconRenderer,int index,int negatingIconIndex,int iconRendererIndex,bool destroyIcosnAtTheEnd)
    {
        while(Vector2.Distance(iconRenderer.IconSlot.Slot.position, iconRenderer.SpriteRenderer.transform.position)>0.1f)
        {
            iconRenderer.SpriteRenderer.transform.position = Vector2.MoveTowards(iconRenderer.SpriteRenderer.transform.position, iconRenderer.IconSlot.Slot.transform.position, _spellIconMoveSpeed * Time.deltaTime);
            yield return null;
        }
        iconRenderer.SpriteRenderer.transform.position = iconRenderer.IconSlot.Slot.transform.position;
        _movingSpellIconsCors.Remove(_movingSpellIconsCors.Find(x => x.index == index));


        if (_removeOldCorutines)
        {
            if (_corIndex == 120)
            {
                _removeOldCorutines = false;
                _corIndex = 0;
                _movingSpellIconsCors.RemoveAll(x => x.index <= 80);
            }
        }
        else
        {
            if (_corIndex == 40)
            {
                _movingSpellIconsCors.RemoveAll(x => x.index >= 80);
                _removeOldCorutines = true;
            }
        }
        //if (destroyIcosnAtTheEnd)
        //{
        //    _iconsRenderers[negatingIconIndex].SpriteRenderer.gameObject.SetActive(false);
        //    _iconsRenderers[iconRendererIndex].SpriteRenderer.gameObject.SetActive(false);
        //    _spellIconDestoryer.transform.position = _iconsRenderers[negatingIconIndex].IconSlot.Slot.position;
        //    _spellIconDestoryer.SetSpritesToDestroy(_iconsRenderers[negatingIconIndex].SpriteRenderer.sprite, _iconsRenderers[iconRendererIndex].SpriteRenderer.sprite);
        //    _spellIconDestoryer.gameObject.SetActive(true);
        //    _iconsRenderers[negatingIconIndex].SpriteRenderer.sortingOrder = 1;
        //    _iconsRenderers[iconRendererIndex].SpriteRenderer.sortingOrder = 1;
        //    _iconsRenderers[negatingIconIndex].IconSlot.AssignedIconRenderer = null;
        //    _iconsRenderers[iconRendererIndex].IconSlot.AssignedIconRenderer = null;
        //    _iconsRenderers[negatingIconIndex].IconSlot.Spell = _physicalSpell;
        //    _iconsRenderers[iconRendererIndex].IconSlot.Spell = _physicalSpell;
        //    _iconsRenderers[negatingIconIndex].IconSlot = null;
        //    _iconsRenderers[iconRendererIndex].IconSlot = null;
        //    for (int i = 0; i < _movingSpellIconsCors.Count;i++)
        //    {
        //        if(_movingSpellIconsCors[i].Cor!=null) StopCoroutine(_movingSpellIconsCors[i].Cor);
        //    }
        //    AdjustIcons();
        //}
        //_iconsSlots.Find(x => x.Slot == spellIconSlot).AssignedIconRenderer = null;
        //_iconsRenderers[iconRendererIndex].IconSlot = null;




    }
}
