using System.Collections.Generic;
using UnityEngine;

public abstract class ContinousAttack 
{
    protected List<IDamagable> _damageablesInRange = new List<IDamagable>();
    public abstract void StartAttack();
    public abstract void Attack();
    public abstract void EndAttack();

}
