using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomTwoTargetsStrategy : ITargetStrategy {
    private System.Random random = new System.Random();

    public List<UnitStats> SelectTargets(UnitStats[] units) {
        List<UnitStats> validTargets = units.Where(unit => unit != null).ToList();
        if (validTargets.Count < 2) {
            return validTargets;
        }
        List<UnitStats> selectedTargets = new List<UnitStats>();
        while (selectedTargets.Count < 2) {
            UnitStats target = validTargets[random.Next(validTargets.Count)];
            selectedTargets.Add(target);
        }
        return selectedTargets;
    }
}
