using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    public Hero hero;
    public float atk;
    public float def;
    public float hp;
    public float maxHP;
    public TMP_Text currentHP;
    public Slider currentHPBar;

    public void Initialize(Hero hero)
    {
        atk = hero.atk;
        def = hero.def;
        hp = hero.hp;
        maxHP = hp;
        UpdateHealthUI();
    }

    private void UpdateHealthUI () {
        currentHP.SetText(hp.ToString("F0"));
        currentHPBar.value = hp / maxHP;
    }

    public void TakeDamage(float damage) {
        hp -= damage;
        if (hp < 0) hp = 0;
        UpdateHealthUI();
    }


}
