using UnityEngine;

public class ToogleDamageStrategy : IDamageStrategy {
    private float damageMultiplier;

    public ToogleDamageStrategy(float multiplier) {
        damageMultiplier = multiplier;
    }

    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float baseDamage = attacker.atk - defender.def;

        if (defender.atk > attacker.atk)
        {
            baseDamage *= damageMultiplier;
        }

        float damage = baseDamage;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}