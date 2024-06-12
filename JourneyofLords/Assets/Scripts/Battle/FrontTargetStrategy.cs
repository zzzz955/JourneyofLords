using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FrontTargetStrategy : ITargetStrategy {
    public UnitStats SelectTarget(UnitStats[] units) {
        return units.FirstOrDefault(unit => unit != null);
    }
}