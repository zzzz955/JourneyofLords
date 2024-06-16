using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicDamageStrategy : IDamageStrategy {
    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float healthFactorAttacker = Mathf.Max(0.5f, attacker.hp / attacker.maxHP);
        float healthFactorDefender = Mathf.Max(0.5f, defender.hp / defender.maxHP);
        float damage = attacker.atk * healthFactorAttacker - defender.def * healthFactorDefender;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}