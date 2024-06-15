using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighDefenseTargetStrategy : ITargetStrategy {
    public List<UnitStats> SelectTargets(UnitStats[] units) {
        var target = units.Where(unit => unit != null && !unit.isDead()).OrderByDescending(unit => unit.def).FirstOrDefault();
        return target != null ? new List<UnitStats> { target } : new List<UnitStats>();
    }
}
