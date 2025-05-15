using System;
using UnityEngine;

public abstract class EnemyContext
{
    public Action<EnemyState> ChangeEnemyState;
    public Transform playerTransform;
    public Transform enemyTransform;
    public AnimationManager animMan;
    public EnemyBasicStats stats;
    public MonoBehaviour coroutineHolder;

}
