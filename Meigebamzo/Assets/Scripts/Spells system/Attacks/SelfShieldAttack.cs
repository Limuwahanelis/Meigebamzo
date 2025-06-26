using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfShieldAttack : ContinousAttack
{
    private List<SpriteRenderer> _selfShieldSprites;
    private List<SpriteRenderer> _auraShieldSprites;
    private MonoBehaviour _corutineHolder;
    private List<MaterialPropertyBlock> _selfShieldPropertyBlocks= new List<MaterialPropertyBlock>() {new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock() };
    private float _baseSkillDuration = 2f;
    private HealthBarF _shieldBar;
    private ElementalAffliction _elementalAffliction;
    private float _baseRaius = 0.04f;
    private Coroutine _cor;
    List<PlayerElementalSpells> _spellsInShield= new List<PlayerElementalSpells>();
    public SelfShieldAttack(List<SpriteRenderer> selfShieldSprites, List<SpriteRenderer> auraShieldSprites, MonoBehaviour corutineHolder, HealthBarF shieldBar, ElementalAffliction elementalAffliction)
    {
        _selfShieldSprites = selfShieldSprites;
        _auraShieldSprites = auraShieldSprites;
        _corutineHolder = corutineHolder;
        _shieldBar = shieldBar;
        _elementalAffliction = elementalAffliction;
    }
    public override void SetSpells(List<PlayerElementalSpells> spells)
    {
        for(int i=0;i<_selfShieldSprites.Count;i++)
        {
            _selfShieldSprites[i].enabled = false;
        }
        _spells.Clear();
        base.SetSpells(spells);
        _spellsInShield.Clear();
        _spellsInShield = _spells.FindAll(x => x.BasicElement.Element != Elements.Element.SHIELD);
        for (int i = 0; i < _spellsInShield.Count; i++)
        {
            _elementalAffliction.AddElementFromSelfShield(_spellsInShield[i].BasicElement);
            _selfShieldPropertyBlocks[i].SetColor("_Color", _spellsInShield[i].BasicElement.AssociatedColor);
            _selfShieldPropertyBlocks[i].SetFloat("_Radius", 0.04f+0.02f*i);
        }
    }
    public override void StartAttack()
    {
        //_auraShieldSprite.SetPropertyBlock(_selfShieldPropertyBlock);

        if (_cor!=null)
        {
            _corutineHolder.StopCoroutine(_cor);
        }
        _shieldBar.SetMaxHealth(2f);
        for(int i=0;i<_spellsInShield.Count;i++)
        {
            _selfShieldSprites[i].enabled = true;
            _selfShieldSprites[i].SetPropertyBlock(_selfShieldPropertyBlocks[i]);
        }
        _shieldBar.gameObject.SetActive(true);
        _cor =_corutineHolder.StartCoroutine(ShieldCor());
    }
    public override void Attack()
    {
      
    }

    public override void EndAttack()
    {
        
    }

    private IEnumerator ShieldCor()
    {
        float t = _baseSkillDuration;
        while(t>=0)
        {
            t -= Time.deltaTime;
            _shieldBar.ReduceHP(Time.deltaTime);
            yield return null;
        }
        _shieldBar.SetHealth(0);
        for(int i=0;i< _spellsInShield.Count;i++)
        {
            _selfShieldSprites[i].enabled = false;

        }
        for (int i = 0; i < _spellsInShield.Count; i++)
        {
            _elementalAffliction.RemoveElementFromSelfShield(_spellsInShield[i].BasicElement);
        }
        
        _shieldBar.gameObject.SetActive(false);
        _spellsInShield.Clear();
        _spells.Clear();
    }
}

