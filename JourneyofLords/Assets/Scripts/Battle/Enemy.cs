using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public Vector2Int position { get; set; }
    public Hero hero { get; set; }

    public Enemy(Vector2Int position, Hero hero)
    {
        this.position = position;
        this.hero = hero;
    }

    public int Health
    {
        get { return hero.Health; }
        set { hero.Health = value; }
    }

    public int Atk => (int)hero.atk;
    public int Def => (int)hero.def;

    public bool IsAlive()
    {
        return hero.IsAlive();
    }
}
