using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
