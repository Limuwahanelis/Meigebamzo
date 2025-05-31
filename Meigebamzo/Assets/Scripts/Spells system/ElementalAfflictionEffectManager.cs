using System.Collections;
using UnityEngine;


public class ElementalAfflictionEffectManager : MonoBehaviour
{
    [SerializeField] ElementalAffliction _elementalAffliciotn;
    [SerializeField] ElementalAfflictionEffect _burningEffect;
    [SerializeField] HealthSystem _damagable;

    DamageInfo _damageInfo= new DamageInfo();
    private Coroutine _effectCor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _elementalAffliciotn.OnAfflictedByElement.AddListener(AffectedByElement);
        _elementalAffliciotn.OnAfflictedRemoved.AddListener(StopEffect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AffectedByElement(Elements.Element element)
    {
        if (_effectCor != null) StopCoroutine(_effectCor);
        if (element == Elements.Element.FIRE) _effectCor = StartCoroutine(FireAfflictionCor(_burningEffect.Duration));
    }
    private void StopEffect(Elements.Element element)
    {
        if (_effectCor != null) StopCoroutine(_effectCor);
    }
    IEnumerator FireAfflictionCor(float duration)
    {
        float t = 0;
        int i = 1;
        _damageInfo.dmgPosition=transform.position;
        _damageInfo.dmg = _burningEffect.Damage;
        _damageInfo.basicElement = _burningEffect.Element;
        while (t < duration)
        {
            if (t>=i*_burningEffect.DamageCooldown)
            {
                _damagable.TakeDamage(_damageInfo);
                i++;
            }
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        _elementalAffliciotn.OnAfflictedByElement.RemoveListener(AffectedByElement);
        _elementalAffliciotn.OnAfflictedRemoved.RemoveListener(StopEffect);
    }

    private void Reset()
    {
        _elementalAffliciotn = GetComponent<ElementalAffliction>();
        _damagable = GetComponent<HealthSystem>();
    }
}
