using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour {
    public IDamageStrategy DamageStrategy { get; set; }
    public ITargetStrategy TargetStrategy { get; set; }
    private int turnCounter = 0;
    private IDamageStrategy initialDamageStrategy;
    private ITargetStrategy initialTargetStrategy;
    private Hero currentHero;
    public int initialAttackCount = 1;

    public void Initialize(Hero hero) {
        currentHero = hero;
        turnCounter = 0; // 턴 카운터 초기화
        switch (hero.index) {
            case 30:
                initialDamageStrategy = new SpecialDamageStrategy(1.10f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 31:
                initialDamageStrategy = new SpecialDamageStrategy(1.20f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 32:
                initialDamageStrategy = new SpecialDamageStrategy(1.40f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 36:
                initialDamageStrategy = new SpecialDamageStrategy(1.00f);
                initialTargetStrategy = new HighAttackTargetStrategy();
                break;
            case 37:
                initialDamageStrategy = new SpecialDamageStrategy(1.10f);
                initialTargetStrategy = new HighAttackTargetStrategy();
                break;
            case 38:
                initialDamageStrategy = new SpecialDamageStrategy(1.20f);
                initialTargetStrategy = new HighAttackTargetStrategy();
                break;
            case 39:
                initialDamageStrategy = new SpecialDamageStrategy(1.20f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 40:
                initialDamageStrategy = new SpecialDamageStrategy(1.35f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 41:
                initialDamageStrategy = new SpecialDamageStrategy(1.50f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 42:
                initialDamageStrategy = new SpecialDamageStrategy(0.50f);
                initialTargetStrategy = new FrontTargetStrategy();
                initialAttackCount = 2;
                break;
            case 43:
                initialDamageStrategy = new SpecialDamageStrategy(0.60f);
                initialTargetStrategy = new FrontTargetStrategy();
                initialAttackCount = 2;
                break;
            case 44:
                initialDamageStrategy = new SpecialDamageStrategy(0.70f);
                initialTargetStrategy = new FrontTargetStrategy();
                initialAttackCount = 2;
                break;
            case 60:
                initialDamageStrategy = new SpecialDamageStrategy(0.60f);
                initialTargetStrategy = new RandomTargetStrategy();
                initialAttackCount = 3;
                break;
            case 61:
                initialDamageStrategy = new SpecialDamageStrategy(0.70f);
                initialTargetStrategy = new RandomTargetStrategy();
                initialAttackCount = 3;
                break;
            case 62:
                initialDamageStrategy = new SpecialDamageStrategy(0.80f);
                initialTargetStrategy = new RandomTargetStrategy();
                initialAttackCount = 3;
                break;
            default:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
        }

        DamageStrategy = initialDamageStrategy;
        TargetStrategy = initialTargetStrategy;
    }

    public void StartTurn() {
        turnCounter++;
        switch (currentHero.index) {
            case 60:
            case 61:
            case 62:
                if (turnCounter > 1) {
                DamageStrategy = new BasicDamageStrategy();
                TargetStrategy = new FrontTargetStrategy();
                initialAttackCount = 1;
                }
                break;
            default:
                break;
        }
    }

    public void Attack(UnitStats[] enemyUnits) {
        List<UnitStats> targets = TargetStrategy.SelectTargets(enemyUnits);
        foreach (var target in targets) {
            if (target != null) {
                float damage = DamageStrategy.CalculateDamage(this.GetComponent<UnitStats>(), target);
                target.TakeDamage(damage);
            }
        }
    }
}
