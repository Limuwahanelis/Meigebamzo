using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesList : MonoBehaviour
{
    public static List<IDamagable> AllEnemiesTransform => _allEnemiesTransform;
    private static List<IDamagable> _allEnemiesTransform = new List<IDamagable>();

    public static void AddEnemy(IDamagable damageable)
    {
        _allEnemiesTransform.Add(damageable);
    }
    public static void RemoveEnemy(IDamagable damageable)
    {
        _allEnemiesTransform.Remove(damageable);
    }

    private void OnDestroy()
    {
        _allEnemiesTransform.Clear();
    }
}
