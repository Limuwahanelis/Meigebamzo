using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ElementSprite : MonoBehaviour
{
    private IObjectPool<ElementSprite> _pool;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUp(float timeToDisappear)
    {
        StartCoroutine(ReturnToPool(timeToDisappear));
    }
    private IEnumerator ReturnToPool(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }

    public void SetPool(ObjectPool<ElementSprite> pool)=> _pool = pool;
    private void ReturnToPool() => _pool.Release(this);
}
