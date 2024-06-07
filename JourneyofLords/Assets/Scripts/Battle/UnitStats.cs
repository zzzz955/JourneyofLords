using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats
{
    public Hero hero;
    public GameObject unitObject;
    public float atk;
    public float def;
    public float hp;
    public float maxHP;

    public UnitStats(Hero hero, GameObject unitObject)
    {
        this.hero = hero;
        this.unitObject = unitObject;
        this.atk = hero.atk;
        this.def = hero.def;
        this.hp = hero.hp;
        this.maxHP = hp;
    }
}
