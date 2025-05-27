using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ElementalAffliction : MonoBehaviour
{
    public Coroutine AssignedAfflictionCorutine {  get; set; }
    public Elements.Element ElementAffectedBy => _elementAffectedBy;
    public List<Elements.Element> ElementsImmuneTo => _elementsImmuneTo;

    [SerializeField] List<Elements.Element> _elementsImmuneTo = new List<Elements.Element>();
    [SerializeField] SpriteRenderer _afflictedElementIcon;
    [SerializeField] ElementSpriteIconManager _iconsManager;
    [SerializeField] float _standardAfflictionDuration=5f;
    [SerializeField] float _negatedAfflictionDuration=1f;

    private Elements.Element _elementAffectedBy;
    private bool _canFireEvent = true;
    public UnityEvent<Elements.Element> OnAfflictedByElement;
    private Coroutine _elementalCor;
    private void Start()
    {
        if(_iconsManager==null) _iconsManager= GameObject.FindAnyObjectByType<ElementSpriteIconManager>();
    }

    public void TrySetElement(BasicElement basicElement)
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
    public void ClearElement()
    {
        _elementAffectedBy = Elements.Element.PHYSICAL;
        if (_afflictedElementIcon) _afflictedElementIcon.enabled = false;
        if (_elementalCor != null)
        {
            StopCoroutine(_elementalCor);
            _elementalCor = null;
        }
        
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
    }
}
