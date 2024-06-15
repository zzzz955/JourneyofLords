using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiAttackStrategy : ITargetStrategy {
    private ITargetStrategy baseStrategy;
    private int attackCount;

    public MultiAttackStrategy(ITargetStrategy strategy, int attackCount) {
        baseStrategy = strategy;
        this.attackCount = attackCount;
    }

    public List<UnitStats> SelectTargets(UnitStats[] units) {
        List<UnitStats> allTargets = new List<UnitStats>();

        for (int i = 0; i < attackCount; i++) {
            var validTargets = units.Where(unit => unit != null && !unit.isDead() && !allTargets.Contains(unit)).ToList();
            if (validTargets.Count == 0) break;

            var selectedTarget = baseStrategy.SelectTargets(validTargets.ToArray()).FirstOrDefault();
            if (selectedTarget != null) {
                allTargets.Add(selectedTarget);
            }
        }

        return allTargets;
    }
}
