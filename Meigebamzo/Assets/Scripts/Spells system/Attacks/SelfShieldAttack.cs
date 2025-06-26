using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfShieldAttack : ContinousAttack
{

    private MonoBehaviour _corutineHolder;
    private List<MaterialPropertyBlock> _selfShieldPropertyBlocks= new List<MaterialPropertyBlock>() {new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock(), new MaterialPropertyBlock() };
    private float _baseSkillDuration = 2f;
    private HealthBarF _shieldBar;
    private ElementalAffliction _elementalAffliction;
    private float _baseRaius = 0.04f;
    private Coroutine _cor;
    private ElementalShieldLogic _elementalShieldLogic= new ElementalShieldLogic();
    List<ElementalSpell> _spellsInShield= new List<ElementalSpell>();
    public SelfShieldAttack( MonoBehaviour corutineHolder, HealthBarF shieldBar, ElementalAffliction elementalAffliction)
    {

        _corutineHolder = corutineHolder;
        _shieldBar = shieldBar;
        _elementalAffliction = elementalAffliction;
    }
    public override void SetSpells(List<PlayerElementalSpells> spells)
    {

        _spells.Clear();
        base.SetSpells(spells);
        _elementalShieldLogic.DisableShield();
        _spellsInShield.Clear();
        _spellsInShield = _spells.Where(x => x.BasicElement.Element != Elements.Element.SHIELD).Select(x => x.BasicElement).ToList();//FindAll(x => x.BasicElement.Element != Elements.Element.SHIELD);
        _elementalShieldLogic.SetUp(_spellsInShield);

       

    }
    public override void StartAttack()
    {
       
        //_auraShieldSprite.SetPropertyBlock(_selfShieldPropertyBlock);

        if (_cor!=null)
        {
            _elementalShieldLogic.DisableShield();
            _corutineHolder.StopCoroutine(_cor);
        }
        _elementalAffliction.AddElementsFromShield(_elementalShieldLogic);
        _shieldBar.SetMaxHealth(2f);
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
        //for(int i=0;i< _spellsInShield.Count;i++)
        //{
        //    _selfShieldSprites[i].enabled = false;

        //}
        //for (int i = 0; i < _spellsInShield.Count; i++)
        //{
        _elementalShieldLogic.DisableShield();
        //}
        
        _shieldBar.gameObject.SetActive(false);
        _spellsInShield.Clear();
        _spells.Clear();
    }
}

