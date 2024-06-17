using UnityEngine;

public class SpecialDamageStrategy : IDamageStrategy {
    private float damageMultiplier;

    public SpecialDamageStrategy(float multiplier) {
        damageMultiplier = multiplier;
    }

    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float baseDamage = attacker.atk - defender.def;
        float damage = baseDamage * damageMultiplier;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}