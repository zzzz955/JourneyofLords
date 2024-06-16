using UnityEngine;

public class SpecialDamageStrategy : IDamageStrategy {
    private float damageMultiplier;

    public SpecialDamageStrategy(float multiplier) {
        damageMultiplier = multiplier;
    }

    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float healthFactorAttacker = Mathf.Max(0.5f, attacker.hp / attacker.maxHP);
        float healthFactorDefender = Mathf.Max(0.5f, defender.hp / defender.maxHP);
        float baseDamage = attacker.atk * healthFactorAttacker - defender.def * healthFactorDefender;
        float damage = baseDamage * damageMultiplier;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}