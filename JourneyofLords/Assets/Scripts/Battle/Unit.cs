using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public IDamageStrategy DamageStrategy { get; set; }
    public ITargetStrategy TargetStrategy { get; set; }

    public void Initialize(IDamageStrategy damageStrategy, ITargetStrategy targetStrategy) {
        DamageStrategy = damageStrategy;
        TargetStrategy = targetStrategy;
    }

    public void Attack(UnitStats[] enemyUnits) {
        UnitStats target = TargetStrategy.SelectTarget(enemyUnits);
        if (target != null) {
            float damage = DamageStrategy.CalculateDamage(this.GetComponent<UnitStats>(), target);
            target.TakeDamage(damage);
        }
    }
}