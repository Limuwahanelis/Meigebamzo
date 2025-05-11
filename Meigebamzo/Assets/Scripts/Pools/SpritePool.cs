using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Search;

// Change "PoolItem" to type of object to make pool for
public class  SpritePool: MonoBehaviour
{
    [SerializeField] ElementSprite _itemPrefab;
    private ObjectPool<ElementSprite> _pool;

    // Start is called before the first frame update
    void Awake()
    {
        _pool = new ObjectPool<ElementSprite>(CreateItem,OnTakeItemFromPool,OnReturnItemToPool);
    }

    public ElementSprite GetItem()
    {
        return _pool.Get();
    }
    ElementSprite CreateItem()
    {
        ElementSprite item = Instantiate(_itemPrefab);
        item.SetPool(_pool);
        return item;

    }
    void OnTakeItemFromPool(ElementSprite item)
    {
        item.gameObject.SetActive(true);
    }
    void OnReturnItemToPool(ElementSprite item)
    {
        item.gameObject.SetActive(false);
    }
}
