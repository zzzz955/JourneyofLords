using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int level;
    public int needEXP;

    public LevelData(int level, int needEXP)
    {
        this.level = level;
        this.needEXP = needEXP;
    }
}
