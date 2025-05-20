using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMissileSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bonePrefab;
    [SerializeField] Transform[] _spawnPositions;
    [SerializeField] Transform _owner;
    private void Start()
    {
        //InitialSpawn();
    }
    public GameObject SpawnBone(int spawnerPosIndex,Transform parent)
    {
       return Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation, parent);
    }
    public GameObject SpawnBone(int spawnerPosIndex,Transform target,float speed)
    {
        BoneMissile boneMissile = Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation).GetComponent<BoneMissile>();
        boneMissile.SetDirectionAndSpeed(target.position, speed);
        boneMissile.SetOwner(_owner);
        return boneMissile.gameObject;
    }
    public GameObject SpawnBone(int spawnerPosIndex,float angle,Transform parent)
    {
        BoneMissile aa =Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation, parent).GetComponent<BoneMissile>();
        aa.SetRotation(angle);
        return aa.gameObject;
    }
    public void InitialSpawn(int index)
    {
        SpawnBone(index, _spawnPositions[index]);
        SpawnBone(index, 90, _spawnPositions[index]);
        //StartCoroutine(SpawnBoneDelayed(0, 0));
        //StartCoroutine(SpawnBoneDelayed(3.5f, 0));
    }

    IEnumerator SpawnBoneDelayed(float delay,int spawnIndex)
    {
        yield return new WaitForSeconds(delay);
        SpawnBone(spawnIndex,null);
    }
}
