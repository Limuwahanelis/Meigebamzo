using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Search;

// Change "PoolItem" to type of object to make pool for
public class  GameObjectPool: MonoBehaviour
{
    [SerializeField]GameObject _itemPrefab;
    private ObjectPool<GameObject> _pool;

    // Start is called before the first frame update
    void Awake()
    {
        _pool = new ObjectPool<GameObject>(CreateItem,OnTakeItemFromPool,OnReturnItemToPool);
    }
    public void Relaese(GameObject item)
    {
        _pool.Release(item);
    }

    public GameObject GetItem()
    {
        return _pool.Get();
    }
    GameObject CreateItem()
    {
        GameObject item = Instantiate(_itemPrefab);
        //item.SetPool(_pool);
        return item;

    }
    void OnTakeItemFromPool(GameObject item)
    {
        item.SetActive(true);
    }
    void OnReturnItemToPool(GameObject item)
    {
        item.SetActive(false);
    }
}
