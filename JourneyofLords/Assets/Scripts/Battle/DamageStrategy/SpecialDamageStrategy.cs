using UnityEngine;

public class SpecialDamageStrategy : IDamageStrategy {
    private float damageMultiplier;

    public SpecialDamageStrategy(float multiplier) {
        damageMultiplier = multiplier;
    }

    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float damage = attacker.atk * damageMultiplier - defender.def;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}