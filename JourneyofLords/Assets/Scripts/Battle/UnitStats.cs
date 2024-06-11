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
    public GameObject hitDamagePrefab;
    public Canvas unitCanvas;

    void Start() {

    }

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
        ShowDamage(damage);
        UpdateHealthUI();
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
}
