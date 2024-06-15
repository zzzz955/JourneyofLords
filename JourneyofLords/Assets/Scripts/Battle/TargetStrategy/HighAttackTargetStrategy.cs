using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighAttackTargetStrategy : ITargetStrategy {
    public List<UnitStats> SelectTargets(UnitStats[] units) {
        var target = units.Where(unit => unit != null).OrderByDescending(unit => unit.atk).FirstOrDefault();
        return target != null ? new List<UnitStats> { target } : new List<UnitStats>();
    }
}
