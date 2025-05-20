using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform[] _spawnPositions;

    public RotatingObject SpawnObject(int spawnerPosIndex, Transform parent)
    {

        RotatingObject aa = Instantiate(_prefab, _spawnPositions[spawnerPosIndex].position, _prefab.transform.rotation, parent).GetComponent<RotatingObject>();
        return aa;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
