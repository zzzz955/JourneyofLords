using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageStrategy {
    float CalculateDamage(UnitStats attacker, UnitStats defender);
}