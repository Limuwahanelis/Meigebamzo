using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class ElementalAffliction : MonoBehaviour
{
    private class ElementAssignedToSpriteRenderer
    {
        public SpriteRenderer spriteRenderer;
        public Elements.Element Element;
        public MaterialPropertyBlock MaterialPropertyBlock;
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
    public void AddElementsFromShield(ElementalShieldLogic shield)
    {
        shield.OnShieldDisabled+=(RemoveElementsFromShield);

        for (int i=0;i<shield.ElementalSpellsToShieldFrom.Count;i++)
        {
            if (_elementsImmuneToFromShields.Contains(shield.ElementalSpellsToShieldFrom[i].Element))
            {

            }
            else
            {
                Logger.Log($"ADD element: {shield.ElementalSpellsToShieldFrom[i].Element}");
                GameObject go = _shieldObjectsPool.GetItem();
                SpriteRenderer newRenderer = go.GetComponent<SpriteRenderer>();
                go.GetComponent<FollowObject>().SetObjectToFollow(_shieldSpawnTran);
                newRenderer.enabled = true;
                _shieldSprites.Add(newRenderer);
                MaterialPropertyBlock block;
                if (_shieldPropertyBlocks.Count < _shieldSprites.Count)
                {
                    _shieldPropertyBlocks.Add(new MaterialPropertyBlock());
                }
                block = _shieldPropertyBlocks.Find(x => _assignedSpriteRenderers.Find(y => y.MaterialPropertyBlock == x) == null);
                block.SetColor("_Color", shield.ElementalSpellsToShieldFrom[i].AssociatedColor);
                block.SetFloat("_Radius", 0.04f + 0.02f * (_shieldSprites.Count-1));
                newRenderer.SetPropertyBlock(block);
                _assignedSpriteRenderers.Add(new ElementAssignedToSpriteRenderer() { Element = shield.ElementalSpellsToShieldFrom[i].Element, spriteRenderer = newRenderer,MaterialPropertyBlock = block });
                
            }
            _elementsImmuneToFromShields.Add(shield.ElementalSpellsToShieldFrom[i].Element);
        }

           
    }
    public void RemoveElementsFromShield( ElementalShieldLogic shield)
    {
        Logger.Log("remove lements shield");
        shield.OnShieldDisabled-=(RemoveElementsFromShield);
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
                Logger.Log($"REMOVE element: {shield.ElementalSpellsToShieldFrom[i].Element}");
                ElementAssignedToSpriteRenderer elementAssigned = _assignedSpriteRenderers.Find(y => y.Element == shield.ElementalSpellsToShieldFrom[i].Element);
                elementAssigned.spriteRenderer.enabled = false;
                SpriteRenderer spriteRenderer = _shieldSprites.Find(x => x == elementAssigned.spriteRenderer);
                _shieldSprites.Remove(spriteRenderer);
                _shieldObjectsPool.Relaese(spriteRenderer.gameObject); 
                _assignedSpriteRenderers.Remove(elementAssigned);
              
            }
        }
        UpdateShieldRings();



    }
    private void UpdateShieldRings()
    {
        for(int i=0;i< _assignedSpriteRenderers.Count;i++)
        {
            _assignedSpriteRenderers[i].MaterialPropertyBlock.SetFloat("_Radius", 0.04f + 0.02f * (i));
            Logger.Log( _assignedSpriteRenderers[i].MaterialPropertyBlock.GetColor("_Color"));
            _assignedSpriteRenderers[i].spriteRenderer.SetPropertyBlock(_assignedSpriteRenderers[i].MaterialPropertyBlock);
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
