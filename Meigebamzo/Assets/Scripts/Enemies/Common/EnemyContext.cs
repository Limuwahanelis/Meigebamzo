using System;
using UnityEngine;

public abstract class EnemyContext
{
    public Action<EnemyState> ChangeEnemyState;
    public GameObject enemyGameobject;
    public Transform playerTransform;
    public Transform enemyTransform;
    public AnimationManager animMan;
    public EnemyBasicStats stats;
    public MonoBehaviour coroutineHolder;
    public Rigidbody2D enemyRigidBody2D;
    public Rigidbody2D playerRB;
    public SpriteRenderer spriteRenderer;
    public Material burnDeathMat;
    public float timeToBurn;

}
