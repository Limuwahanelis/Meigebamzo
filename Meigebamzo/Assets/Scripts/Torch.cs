using UnityEngine;
using UnityEngine.Events;

public class Torch : MonoBehaviour
{
    public UnityEvent OnFired;
    public UnityEvent OnExtinguished;
    [SerializeField] GameObject _particleEffects;
    [SerializeField] bool _startingState;
    [SerializeField] bool _isGoalToFire;
    private bool _isFired;
    private void Awake()
    {
        _isFired = _startingState;
        _particleEffects.SetActive(_isFired);
        if(_isGoalToFire)
        {
            if (_isFired) OnFired?.Invoke();
        }
        else
        {
            if (!_isFired) OnExtinguished?.Invoke();
        }
    }
    public void SetTorch(Elements.Element element)
    {
        if (element == Elements.Element.WATER)
        {
            _particleEffects.SetActive(false);
            if (_isFired)
            {
                OnExtinguished?.Invoke();
                _isFired = false;
            }
        }
        else if (element == Elements.Element.FIRE)
        {
            _particleEffects.SetActive(true);
            if(!_isFired)
            {
                OnFired?.Invoke();
                _isFired = true;
            }
        }
    }
}
