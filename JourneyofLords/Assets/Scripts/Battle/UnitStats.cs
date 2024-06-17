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

    private float initialAtk;
    private float initialDef;
    private float initialHp;

    public float atkBonus = 1.00f;
    public float defBonus = 1.00f;
    public float hpBonus = 1.00f;
    public float damageReduce = 1.00f;

    public TMP_Text currentHP;
    public Slider currentHPBar;
    public GameObject hitDamagePrefab;
    public Transform stackBar;
    public GameObject stack;
    public Canvas unitCanvas;

    public void Initialize(Hero hero)
    {
        this.hero = hero;
        atk = hero.atk;
        def = hero.def;
        hp = hero.hp;
        maxHP = hp;

        initialAtk = atk;
        initialDef = def;
        initialHp = hp;

        UpdateHealthUI();
    }

    private void UpdateHealthUI () {
        currentHP.SetText(hp.ToString("F0"));
        currentHPBar.value = hp / maxHP;
    }

    public void TakeDamage(float damage) {
        damage *= damageReduce;
        if (hp - damage <= 0) {
            damage = hp;
            hp = 0;
        } else {
            hp -= damage;
        }
        ShowDamage(damage);
        UpdateHealthUI();
    }

    public bool isDead() {
        return hp <= 0;
    }

    private void ShowDamage(float damage)
    {
        GameObject damageText = Instantiate(hitDamagePrefab, unitCanvas.transform);
        TMP_Text damageTextTMP = damageText.GetComponent<TMP_Text>();
        damageTextTMP.text = damage.ToString("F0");
        StartCoroutine(AnimateDamageText(damageText));
    }

    private IEnumerator AnimateDamageText(GameObject damageText)
    {
        Vector3 initialPosition = damageText.transform.localPosition;
        Vector3 targetPosition = initialPosition + new Vector3(0, 1, 0); // 텍스트가 위로 이동
        float duration = 1.0f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            damageText.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(damageText);
    }

    public void IncreaseStats(float atkPercentage, float defPercentage, float hpPercentage)
    {
        atk *= atkPercentage;
        def *= defPercentage;
        hp *= hpPercentage;
        maxHP = hp;
        UpdateHealthUI();
    }

    public void TurnBasedIncreaseADStats(float atkPercentage, float defPercentage) {
        atk += initialAtk * (atkPercentage / 100f);
        def += initialDef * (defPercentage / 100f);
        
        GameObject stackEffect = Instantiate(stack, stackBar);
    }
}
