using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPush : MonoBehaviour
{
    [SerializeField] int _pushforceMult;
    [SerializeField] AnimationManager _animMan;
    private float _windElements;
    private List<Collider2D> _collidedObjects=new List<Collider2D>();
    private Coroutine _pushCor;
    private float _animlength;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_collidedObjects.Contains(collision))
        {
            _collidedObjects.Add(collision);
            if(collision.attachedRigidbody)
            {
                collision.attachedRigidbody.GetComponent<IPushable>().Push(new PushInfo(transform.position,_windElements*_pushforceMult));
            }
        }
    }

    public void ResetCollision()
    {
        _collidedObjects.Clear();
    }

    public void SetPushForce(int windElementsNum)
    {
        _windElements = windElementsNum;
        if (_pushCor != null) StopCoroutine(_pushCor);
        _pushCor= StartCoroutine(Push(windElementsNum));

    }
    IEnumerator Push(int windElementsNum)
    {
        switch(windElementsNum)
        {
            case 1: _animlength = _animMan.GetAnimationLength("Very small wind push");break;
            case 2: _animlength = _animMan.GetAnimationLength("Small wind push"); break;
            case 3: _animlength = _animMan.GetAnimationLength("Medium wind push"); break;
            case 4: _animlength = _animMan.GetAnimationLength("Big Wind push"); break;
        }
        yield return new WaitForSeconds(_animlength);
        ResetCollision();
    }
}
