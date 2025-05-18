using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesList : MonoBehaviour
{
    public List<Transform> AllEnemiesTransform => _allEnemiesTransform;
    private List<Transform> _allEnemiesTransform = new List<Transform>();

    public void AddEnemy(Transform tran)
    {
        _allEnemiesTransform.Add(tran);
    }
}
