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

    private Elements.Element _elementAffectedBy;
    private bool _canFireEvent = true;
    public UnityEvent<Elements.Element> OnAfflictedByElement;
    private Coroutine _elementalCor; 
    private void Start()
    {
        if(_iconsManager==null) GameObject.FindAnyObjectByType<ElementSpriteIconManager>();
    }

    public void TrySetElement(Elements.Element element)
    {
        if (_elementsImmuneTo.Contains(element)) return;
        else if(element==_elementAffectedBy)
        {
           if(_canFireEvent) OnAfflictedByElement?.Invoke(element);
            if (_elementalCor != null)
            {
                StopCoroutine(_elementalCor);
                _elementalCor = StartCoroutine(AfflictionCor());
            }
            return;
        }
        _afflictedElementIcon.gameObject.SetActive(true);
        _elementAffectedBy = element;
        _afflictedElementIcon.sprite = _iconsManager.ElementIconSprites[((int)element)];
        if(_elementalCor!=null)
        {
            StopCoroutine(_elementalCor);
            _elementalCor = StartCoroutine(AfflictionCor());
        }
        else _elementalCor = StartCoroutine(AfflictionCor());
        if (_canFireEvent) OnAfflictedByElement?.Invoke(element);
    }
    public void ClearElement()
    {
        _elementAffectedBy = Elements.Element.PHYSICAL;
        _afflictedElementIcon.gameObject.SetActive(false);
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
