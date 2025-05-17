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

    private Elements.Element _elementAffectedBy;
    private bool _canFireEvent = true;
    public UnityEvent<Elements.Element> OnAfflictedByElement;
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
            return;
        }
        _afflictedElementIcon.gameObject.SetActive(true);
        _elementAffectedBy = element;
        _afflictedElementIcon.sprite = _iconsManager.ElementIconSprites[((int)element)];
        if (_canFireEvent) OnAfflictedByElement?.Invoke(element);
    }
    public void ClearElement()
    {
        _elementAffectedBy = Elements.Element.PHYSICAL;
        _afflictedElementIcon.gameObject.SetActive(false);
    }

    public void SetCanFireAfflictionEvent(bool value)
    {
        _canFireEvent = value;
    }
}
