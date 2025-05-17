using System.Collections.Generic;
using UnityEngine;

public abstract class ContinousAttack 
{
    protected List<PlayerElementalSpells> _spells= new List<PlayerElementalSpells>();
    protected List<IDamagable> _damageablesInRange = new List<IDamagable>();
    public abstract void StartAttack();
    public abstract void Attack();
    public abstract void EndAttack();

    public void SetSpells(List<PlayerElementalSpells>spells)
    {
        _spells.AddRange(spells);
    }

}
