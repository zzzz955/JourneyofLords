using UnityEngine;

public class ToogleDamageStrategy : IDamageStrategy {
    private float damageMultiplier;

    public ToogleDamageStrategy(float multiplier) {
        damageMultiplier = multiplier;
    }

    public float CalculateDamage(UnitStats attacker, UnitStats defender) {
        float healthFactorAttacker = Mathf.Max(0.5f, attacker.hp / attacker.maxHP);
        float healthFactorDefender = Mathf.Max(0.5f, defender.hp / defender.maxHP);
        float baseDamage = attacker.atk * healthFactorAttacker - defender.def * healthFactorDefender;

        if (defender.atk > attacker.atk)
        {
            baseDamage *= damageMultiplier;
        }

        float damage = baseDamage;
        return Mathf.Max(defender.maxHP * 0.01f, damage);
    }
}