using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllTargetStrategy : ITargetStrategy {
    public List<UnitStats> SelectTargets(UnitStats[] units) {
        var targets = units.Where(unit => unit != null && !unit.isDead()).ToList();
        return targets;
    }
}
