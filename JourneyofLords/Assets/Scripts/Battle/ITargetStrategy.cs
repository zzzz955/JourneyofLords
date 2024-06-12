using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetStrategy {
    UnitStats SelectTarget(UnitStats[] units);
}