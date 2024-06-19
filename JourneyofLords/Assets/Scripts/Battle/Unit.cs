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
    private UnitStats stats;
    private float damageIncrease = 1.00f;

    public int initialAttackCount;

    void Awake() {
        stats = GetComponent<UnitStats>();
    }

    public void Initialize(Hero hero) {
        currentHero = hero;
        turnCounter = 0; // 턴 카운터 초기화
        initialAttackCount = 1;

        var battle = FindObjectOfType<Battle>();

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
            case 33:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.05f, 0.05f, 0.00f);
                break;
            case 34:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.08f, 0.08f, 0.00f);
                break;
            case 35:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.12f, 0.12f, 0.00f);
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
            case 45:
                initialDamageStrategy = new ToogleDamageStrategy(1.20f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 46:
                initialDamageStrategy = new ToogleDamageStrategy(1.40f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 47:
                initialDamageStrategy = new ToogleDamageStrategy(1.60f);
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 48:
                initialDamageStrategy = new SpecialDamageStrategy(1.40f);
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.03f, 0.03f, 0.00f);
                break;
            case 49:
                initialDamageStrategy = new SpecialDamageStrategy(1.60f);
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.04f, 0.04f, 0.00f);
                break;
            case 50:
                initialDamageStrategy = new SpecialDamageStrategy(1.80f);
                initialTargetStrategy = new FrontTargetStrategy();
                battle.BonusUpdate(0.05f, 0.05f, 0.00f);
                break;
            case 51:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 52:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 53:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 54:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 55:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 56:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                break;
            case 57:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                stats.damageReduce -= 0.15f;
                break;
            case 58:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                stats.damageReduce -= 0.25f;
                break;
            case 59:
                initialDamageStrategy = new BasicDamageStrategy();
                initialTargetStrategy = new FrontTargetStrategy();
                stats.damageReduce -= 0.40f;
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
            case 63:
                initialDamageStrategy = new SpecialDamageStrategy(1.10f);
                initialTargetStrategy = new BackTargetStrategy();
                break;
            case 64:
                initialDamageStrategy = new SpecialDamageStrategy(1.20f);
                initialTargetStrategy = new BackTargetStrategy();
                break;
            case 65:
                initialDamageStrategy = new SpecialDamageStrategy(1.30f);
                initialTargetStrategy = new BackTargetStrategy();
                break;
            case 66:
                initialDamageStrategy = new SpecialDamageStrategy(0.30f);
                initialTargetStrategy = new AllTargetStrategy();
                break;
            case 67:
                initialDamageStrategy = new SpecialDamageStrategy(0.40f);
                initialTargetStrategy = new AllTargetStrategy();
                break;
            case 68:
                initialDamageStrategy = new SpecialDamageStrategy(0.50f);
                initialTargetStrategy = new AllTargetStrategy();
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
            case 48:
            case 49:
            case 50:
                if (turnCounter > 1) {
                DamageStrategy = new BasicDamageStrategy();
                }
                break;
            case 51:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.03f, 1.03f);
                }
                break;
            case 52:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.04f, 1.04f);
                }
                break;
            case 53:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.05f, 1.05f);
                    Debug.Log($"호출 : {turnCounter}");
                }
                break;
            case 54:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.05f, 1.00f);
                }
                break;
            case 55:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.07f, 1.00f);
                }
                break;
            case 56:
                if (turnCounter <= 5) {
                    stats.TurnBasedIncreaseADStats(1.10f, 1.00f);
                    Debug.Log($"호출 : {turnCounter}");
                }
                break;
            case 60:
            case 61:
            case 62:
                if (turnCounter > 1) {
                DamageStrategy = new BasicDamageStrategy();
                TargetStrategy = new FrontTargetStrategy();
                initialAttackCount = 1;
                }
                break;
            case 66:
            case 67:
            case 68:
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
                damage *= damageIncrease;
                target.TakeDamage(damage);
            }
        }
    }
}
