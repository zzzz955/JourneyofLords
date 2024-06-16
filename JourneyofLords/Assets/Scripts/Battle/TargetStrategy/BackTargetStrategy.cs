using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackTargetStrategy : ITargetStrategy {
    public List<UnitStats> SelectTargets(UnitStats[] units) {
        var reversedUnits = units.Reverse().ToArray();
        var target = reversedUnits.FirstOrDefault(unit => unit != null && !unit.isDead());
        return target != null ? new List<UnitStats> { target } : new List<UnitStats>();
    }
}