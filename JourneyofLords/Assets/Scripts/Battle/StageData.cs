using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int level;
    public List<Enemy> enemies;

    public StageData(int level, List<Enemy> enemies)
    {
        this.level = level;
        this.enemies = enemies;
    }
}

public class Enemy
{
    public int position;
    public Hero hero;

    public Enemy(int position, Hero hero)
    {
        this.position = position;
        this.hero = hero;
    }
}