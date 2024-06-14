using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public IDamageStrategy DamageStrategy { get; set; }
    public ITargetStrategy TargetStrategy { get; set; }
    private int turnCounter = 0;
    private IDamageStrategy initialDamageStrategy;
    private ITargetStrategy initialTargetStrategy;
    private Hero currentHero;

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
            case 40:
                initialDamageStrategy = new SpecialDamageStrategy(1.20f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 41:
                initialDamageStrategy = new SpecialDamageStrategy(1.35f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 42:
                initialDamageStrategy = new SpecialDamageStrategy(1.50f);
                initialTargetStrategy = new HighDefenseTargetStrategy();
                break;
            case 60:
                initialDamageStrategy = new SpecialDamageStrategy(0.60f);
                initialTargetStrategy = new RandomTwoTargetsStrategy();
                break;
            case 61:
                initialDamageStrategy = new SpecialDamageStrategy(0.70f);
                initialTargetStrategy = new RandomTwoTargetsStrategy();
                break;
            case 62:
                initialDamageStrategy = new SpecialDamageStrategy(0.80f);
                initialTargetStrategy = new RandomTwoTargetsStrategy();
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
                }
                break;
            default:
                break;
        }
    }

    public void Attack(UnitStats[] enemyUnits) {
        List<UnitStats> targets = TargetStrategy.SelectTargets(enemyUnits);

        foreach (var target in targets) {
            float damage = DamageStrategy.CalculateDamage(this.GetComponent<UnitStats>(), target);
            target.TakeDamage(damage);
        }
    }
}
