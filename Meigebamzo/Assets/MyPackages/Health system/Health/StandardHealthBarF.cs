using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StandardHealthBarF : HealthBarF
{
    private MaterialPropertyBlock _propertyBlock;
    private Renderer _renderer;
    [SerializeField] float _maxHP = 100;
    [SerializeField] bool _lazyHealthBar;
    [SerializeField] float _timeToMoveHealth;
    [SerializeField] float _lazyHPLostPerSecond;
    private float _lazyHP;
    private float _currentHP;
    private bool _initialized = false;
    bool _isReducingHP;
    private void Awake()
    {
        Initialize();
    }
    public override void Initialize()
    {
        if (_initialized) return;
        if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _renderer.SetPropertyBlock(_propertyBlock);
        _lazyHP = _currentHP = _maxHP;
        _initialized = true;
    }

    public override void SetHealth(float hp)
    {
        _lazyHP = _currentHP = hp;
        _propertyBlock.SetFloat("_Value", hp / _maxHP);
        _propertyBlock.SetFloat("_LazyFillValue", _lazyHP / _maxHP);
        _renderer.SetPropertyBlock(_propertyBlock);

    }
    public override void SetMaxHealth(float value)
    {
        _maxHP = value;
        SetHealth(_maxHP);
    }
    public override void ReduceHP(float value)
    {
        StartCoroutine(HealthReduceHealthCor());
        _currentHP -= value;

        _propertyBlock.SetFloat("_Value", _currentHP / _maxHP);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
    IEnumerator HealthReduceHealthCor()
    {
        if (_isReducingHP) yield break;
        _isReducingHP = true;
        float time = 0;
        while (time <= _timeToMoveHealth)
        {
            time += Time.deltaTime;
            yield return null;
        }
        while (_lazyHP > _currentHP)
        {
            _lazyHP -= Time.deltaTime * _lazyHPLostPerSecond;
            _lazyHP = math.clamp(_lazyHP, _currentHP, _maxHP);
            _propertyBlock.SetFloat("_LazyFillValue", _lazyHP / _maxHP);
            _renderer.SetPropertyBlock(_propertyBlock);
            yield return null;
        }
        _isReducingHP = false;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(StandardHealthBarF))]
    public class HPEditor : Editor
    {
        StandardHealthBar _instance;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("hp"))
            {
                (target as StandardHealthBar).ReduceHP(10);
            }
        }
    }
#endif
}
