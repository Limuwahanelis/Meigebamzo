using UnityEngine;

public class ReaperCombat : MonoBehaviour
{
    [SerializeField] BoneMissileSpawner _boneMissileSpawner;
    [SerializeField] RotatingObjectsSpawner _rotatingObjectsSpawner;
    public void SpawnBone(int spawnIndex,Transform parent,float speed)
    {
        _boneMissileSpawner.SpawnBone(spawnIndex, parent, speed);
    }

    public void SpawnObject(int spawnerIndex,Transform parent)
    {
        _rotatingObjectsSpawner.SpawnObject(spawnerIndex, parent);
    }
}
