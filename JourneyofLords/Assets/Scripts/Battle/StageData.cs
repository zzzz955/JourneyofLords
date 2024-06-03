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
    public Vector2Int position;
    public Hero hero;

    public Enemy(Vector2Int position, Hero hero)
    {
        this.position = position;
        this.hero = hero;
    }
}