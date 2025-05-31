using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{

    public int dmg;
    public Vector3 dmgPosition;
    public Collider2D[] involvedColliders;
    public BasicElement basicElement;
    public DamageInfo(int dmg,Vector3 dmgPosition, BasicElement element, Collider2D[] colliders=null) 
    {
        this.dmg = dmg;
        this.dmgPosition = dmgPosition;
        involvedColliders = colliders;
        this.basicElement = element;
    }
}
