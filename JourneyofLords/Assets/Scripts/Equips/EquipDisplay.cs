using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class EquipDisplay : MonoBehaviour
{
    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText;
    public GameObject popupEquipPrefab;
    public GameObject popupOwnEquipPrefab;

    private float tempAtk;
    private float tempDef;
    private float tempHp;


    public Equip currentEquip { get; private set; }

    public void SetEquitData(Equip equip)
    {
        currentEquip = equip;
        tempAtk = equip.atk;
        tempDef = equip.def;
        tempHp = equip.hp;

        // 스프라이트 설정
        if (spriteImage != null) {
            Sprite sprite1 = Resources.Load<Sprite>(equip.spriteName);
            if (sprite1 != null) {spriteImage.sprite = sprite1;}
        }

        if (levelText != null) {levelText.SetText(equip.level.ToString());}

        // if (lvImage != null) {
        //     Sprite sprite2 = Resources.Load<Sprite>(equip.);
        //     if (sprite2 != null) {lvImage.sprite = sprite2;}
        // }

        // if (gradeImage != null) {
        //     Sprite sprite3 = Resources.Load<Sprite>(equip.ToString());
        //     if (sprite3 != null) {gradeImage.sprite = sprite3;}
        // }
    }

    public void ShowOwnPopupEquip() {
        GameObject popup = Instantiate(popupOwnEquipPrefab, transform.parent.parent); // 부모를 설정 (메인 캔버스 또는 루트)
        PopupEquipDisplay popupEquipDisplay = popup.GetComponent<PopupEquipDisplay>();

        if (popupEquipDisplay != null)
        {
            popupEquipDisplay.SetEquipData(currentEquip, tempAtk, tempDef, tempHp);
        }
    }

    public void ShowPopupEquip()
    {
        GameObject popup = Instantiate(popupEquipPrefab, transform.parent.parent); // 부모를 설정 (메인 캔버스 또는 루트)
        PopupEquipDisplay popupEquipDisplay = popup.GetComponent<PopupEquipDisplay>();

        if (popupEquipDisplay != null)
        {
            popupEquipDisplay.SetEquipData(currentEquip, tempAtk, tempDef, tempHp);
        }
    }

    // public void UpdateStats(float newAtk, float newDef, float newHP, float atkBonusPercentage, float defBonusPercentage, float hpBonusPercentage)
    // {
    //     tempAtk = newAtk;
    //     tempDef = newDef;
    //     tempHp = newHP;
        
    //     if (atkBonusObject != null)
    //     {
    //         TMP_Text atkBonusText = atkBonusObject.GetComponentInChildren<TMP_Text>();
    //         if (atkBonusText != null) { atkBonusText.SetText($"+{atkBonusPercentage:F2}%"); }
    //     }

    //     if (defBonusObject != null)
    //     {
    //         TMP_Text defBonusText = defBonusObject.GetComponentInChildren<TMP_Text>();
    //         if (defBonusText != null) { defBonusText.SetText($"+{defBonusPercentage:F2}%"); }
    //     }

    //     if (hpBonusObject != null)
    //     {
    //         TMP_Text hpBonusText = hpBonusObject.GetComponentInChildren<TMP_Text>();
    //         if (hpBonusText != null) { hpBonusText.SetText($"+{hpBonusPercentage:F2}%"); }
    //     }
    // }
}
