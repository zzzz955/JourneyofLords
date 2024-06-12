using System.Collections.Generic;
using UnityEngine;

public interface ITargetStrategy {
    List<UnitStats> SelectTargets(UnitStats[] units);
}
