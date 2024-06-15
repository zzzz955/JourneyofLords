using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomTargetStrategy : ITargetStrategy {
    private System.Random random = new System.Random();

    public List<UnitStats> SelectTargets(UnitStats[] units) {
        List<UnitStats> validTargets = units.Where(unit => unit != null && !unit.isDead()).ToList();
        if (validTargets.Count == 0) {
            return new List<UnitStats>();
        }
        
        UnitStats target = validTargets[random.Next(validTargets.Count)];
        return new List<UnitStats> { target };
    }
}
