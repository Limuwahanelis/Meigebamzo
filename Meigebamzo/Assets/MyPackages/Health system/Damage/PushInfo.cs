using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushInfo
{
    public Vector3 pushPosition;
    public Collider2D[] involvedColliders;
    public float pushForce;

    public PushInfo(Vector3 pushpos, Collider2D[] involvedColliders=null) 
    {
        this.pushPosition = pushpos;
        this.involvedColliders = involvedColliders;
    }
    public PushInfo(Vector3 pushpos, float pushForce)
    {
        this.pushPosition = pushpos;
        this.pushForce = pushForce;

    }
}
