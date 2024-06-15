using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : ITargetStrategy {
    private ITargetStrategy baseStrategy;

    public DoubleAttack(ITargetStrategy strategy) {
        baseStrategy = strategy;
    }

    public List<UnitStats> SelectTargets(UnitStats[] units) {
        // 동일한 맨 앞의 적을 두 번 타겟팅
        var firstTargets = baseStrategy.SelectTargets(units);
        var allTargets = new List<UnitStats>();
        if (firstTargets.Count > 0) {
            var firstTarget = firstTargets[0];
            allTargets.Add(firstTarget);

            // 첫 번째 타겟이 사망했는지 확인하고 새로운 타겟을 선택
            if (firstTarget.isDead()) {
                var secondTargets = baseStrategy.SelectTargets(units);
                if (secondTargets.Count > 0) {
                    var secondTarget = secondTargets[0];
                    if (secondTarget != firstTarget) {
                        allTargets.Add(secondTarget);
                    }
                }
            } else {
                allTargets.Add(firstTarget);
            }
        }

        return allTargets;
    }
}
