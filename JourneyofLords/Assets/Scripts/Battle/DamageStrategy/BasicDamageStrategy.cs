using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicDamageStrategy : IDamageStrategy {
    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float damage = attacker.atk - defender.def;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}