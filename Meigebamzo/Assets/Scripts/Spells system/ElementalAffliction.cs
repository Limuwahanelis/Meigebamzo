using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ElementalAffliction : MonoBehaviour
{
    private class ElementAssignedToSpriteRenderer
    {
        public SpriteRenderer spriteRenderer;
        public Elements.Element Element;
    }
    public Coroutine AssignedAfflictionCorutine {  get; set; }
    public Elements.Element ElementAffectedBy => _elementAffectedBy;
    public List<Elements.Element> ElementsImmuneTo => _elementsImmuneTo.Concat(_elementsImmuneToFromShields).ToList();
    public Transform ShieldSpawnTran => _shieldSpawnTran;

    [SerializeField] List<Elements.Element> _elementsImmuneTo = new List<Elements.Element>();
    [SerializeField] SpriteRenderer _afflictedElementIcon;
    [SerializeField] ElementSpriteIconManager _iconsManager;
    [SerializeField] float _standardAfflictionDuration=5f;
    [SerializeField] float _negatedAfflictionDuration=1f;
    [SerializeField] List<SpriteRenderer> _shieldSprites= new List<SpriteRenderer>();
    [SerializeField] GameObjectPool _shieldObjectsPool;
    [SerializeField] Transform _shieldSpawnTran;

    private List<Elements.Element> _elementsImmuneToFromShields= new List<Elements.Element>();
    private List<ElementAssignedToSpriteRenderer> _assignedSpriteRenderers = new List<ElementAssignedToSpriteRenderer>();
    private Elements.Element _elementAffectedBy;
    private bool _canFireEvent = true;
    public UnityEvent<Elements.Element> OnAfflictedByElement;
    public UnityEvent<Elements.Element> OnAfflictedRemoved;
    private Coroutine _elementalCor;
    private List<MaterialPropertyBlock> _shieldPropertyBlocks = new List<MaterialPropertyBlock>();

    private void Start()
    {
        if(_iconsManager==null) _iconsManager= FindAnyObjectByType<ElementSpriteIconManager>();
        if(_shieldObjectsPool == null) _shieldObjectsPool= FindAnyObjectByType<GameObjectPool>();
    }

    public void TrySetElement(ElementalSpell basicElement)
    {
        if (_elementsImmuneTo.Contains(basicElement.Element)) return;
        if (basicElement.Element == Elements.Element.ELECTRICITY) return;
        if (basicElement.Element == Elements.Element.WIND) return;
        if (basicElement.Element == Elements.Element.PHYSICAL) return;
        else if (basicElement.Element == _elementAffectedBy)
        {
            if (_canFireEvent) OnAfflictedByElement?.Invoke(basicElement.Element);
            if (_elementalCor != null)
            {
                StopCoroutine(_elementalCor);
                _elementalCor = StartCoroutine(AfflictionCor());
            }
            return;
        }
        if (_afflictedElementIcon)
        {
            _afflictedElementIcon.enabled = true;
            _afflictedElementIcon.sprite = _iconsManager.ElementIconSprites[((int)basicElement.Element)];
        }
        _elementAffectedBy = basicElement.Element;
        if(_elementalCor!=null)
        {
            StopCoroutine(_elementalCor);
            _elementalCor = StartCoroutine(AfflictionCor());
        }
        else _elementalCor = StartCoroutine(AfflictionCor());
        if (_canFireEvent) OnAfflictedByElement?.Invoke(basicElement.Element);
    }
    public void AddElementFromSelfShield(ElementalSpell elementalSpell)
    {
       
        _elementsImmuneToFromShields.Add(elementalSpell.Element);
        //_shieldSprites.Add( _shieldObjectsPool.GetItem().GetComponent<SpriteRenderer>());
        //if(_shieldPropertyBlocks.Count<_shieldSprites.Count)
        //{
        //    _shieldPropertyBlocks.Add(new MaterialPropertyBlock());
        //}
        //_shieldPropertyBlocks[_shieldPropertyBlocks.Count-1].SetColor("_Color", elementalSpell.AssociatedColor);
        //_shieldPropertyBlocks[_shieldPropertyBlocks.Count-1].SetFloat("_Radius", 0.04f + 0.02f * _shieldPropertyBlocks.Count-1);
        //_shieldSprites[_shieldSprites.Count-1].SetPropertyBlock(_shieldPropertyBlocks[_shieldPropertyBlocks.Count-1]);
    }
    public void RemoveElementFromSelfShield(ElementalSpell elementalSpell)
    {
        _elementsImmuneToFromShields.Remove(elementalSpell.Element);
    }
    public void AddElementsFromShield(ElementalShield shield)
    {
        shield.OnShieldDisabled.AddListener(RemoveElementsFromShield);

        for (int i=0;i<shield.ElementalSpellsToShieldFrom.Count;i++)
        {
            if (_elementsImmuneToFromShields.Contains(shield.ElementalSpellsToShieldFrom[i].Element))
            {

            }
            else
            {
                GameObject go = _shieldObjectsPool.GetItem();
                SpriteRenderer newRenderer = go.GetComponent<SpriteRenderer>();
                go.GetComponent<FollowObject>().SetObjectToFollow(_shieldSpawnTran);
                newRenderer.enabled = true;
                _shieldSprites.Add(newRenderer);
                _assignedSpriteRenderers.Add(new ElementAssignedToSpriteRenderer() { Element = shield.ElementalSpellsToShieldFrom[i].Element, spriteRenderer = newRenderer });
                if (_shieldPropertyBlocks.Count < _shieldSprites.Count)
                {
                    _shieldPropertyBlocks.Add(new MaterialPropertyBlock());
                }
                _shieldPropertyBlocks[_shieldPropertyBlocks.Count - 1].SetColor("_Color", shield.ElementalSpellsToShieldFrom[i].AssociatedColor);
                _shieldPropertyBlocks[_shieldPropertyBlocks.Count - 1].SetFloat("_Radius", 0.04f + 0.02f * (i));
                _shieldSprites[_shieldSprites.Count-1].SetPropertyBlock(_shieldPropertyBlocks[_shieldPropertyBlocks.Count - 1]);
            }
            _elementsImmuneToFromShields.Add(shield.ElementalSpellsToShieldFrom[i].Element);
        }

           
    }
    public void RemoveElementsFromShield( ElementalShield shield)
    {
        Logger.Log("remove lements shield");
        shield.OnShieldDisabled.RemoveListener(RemoveElementsFromShield);
        List<Elements.Element> toRemove = shield.ElementalSpellsToShieldFrom.Select(x => x.Element).ToList();
        for (int i = 0; i < toRemove.Count; i++)
        {
            _elementsImmuneToFromShields.Remove(toRemove[i]);
        }
        for (int i = 0; i < shield.ElementalSpellsToShieldFrom.Count; i++)
        {
            if (_elementsImmuneToFromShields.Contains(shield.ElementalSpellsToShieldFrom[i].Element))
            {

            }
            else
            {
                ElementAssignedToSpriteRenderer elementAssigned = _assignedSpriteRenderers.Find(y => y.Element == shield.ElementalSpellsToShieldFrom[i].Element);
                elementAssigned.spriteRenderer.enabled = false;
                SpriteRenderer spriteRenderer = _shieldSprites.Find(x => x == elementAssigned.spriteRenderer);
                _shieldSprites.Remove(spriteRenderer);
                _shieldObjectsPool.Relaese(spriteRenderer.gameObject); 
                _assignedSpriteRenderers.Remove(elementAssigned);
            }
        }

        

    }
    public void ClearElement()
    {
        if (_afflictedElementIcon) _afflictedElementIcon.enabled = false;
        if (_elementalCor != null)
        {
            StopCoroutine(_elementalCor);
            OnAfflictedRemoved?.Invoke(_elementAffectedBy);
            _elementalCor = null;
        }
        _elementAffectedBy = Elements.Element.PHYSICAL;
        
    }

    public void SetCanFireAfflictionEvent(bool value)
    {
        _canFireEvent = value;
    }

    IEnumerator AfflictionCor()
    {
        yield return new WaitForSeconds(_standardAfflictionDuration);
        ClearElement();
    }
    private void Reset()
    {
        _iconsManager = FindAnyObjectByType<ElementSpriteIconManager>();
        _shieldObjectsPool = FindAnyObjectByType<GameObjectPool>();
    }
}
