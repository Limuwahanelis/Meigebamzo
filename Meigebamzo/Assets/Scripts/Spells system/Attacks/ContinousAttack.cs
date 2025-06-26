using System.Collections.Generic;
using UnityEngine;

public abstract class ContinousAttack 
{
    public bool IsContinous { get; protected set; }
    protected List<PlayerElementalSpells> _spells= new List<PlayerElementalSpells>();
    protected List<IDamagable> _damageablesInRange = new List<IDamagable>();
    protected ElementalSpell _basicElement;
    public abstract void StartAttack();
    public abstract void Attack();
    public abstract void EndAttack();

    public virtual void SetSpells(List<PlayerElementalSpells>spells)
    {
        _spells.AddRange(spells);
    }

}
