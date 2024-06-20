using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;


public class PopupEquipDisplay : MonoBehaviour
{
    public Image spriteImage;
    public TMP_Text levelText;
    public TMP_Text equipNameText;
    public TMP_Text gradeText;
    public TMP_Text atkText;
    public TMP_Text defText;
    public TMP_Text hpText;
    public TMP_Text descriptsText;

    public void SetEquipData(Equip equip, float tempAtk, float tempDef, float tempHp)
    {
        // 스프라이트 설정
        Sprite sprite1 = Resources.Load<Sprite>(equip.spriteName);
        if (sprite1 != null) { spriteImage.sprite = sprite1; }

        // 텍스트 필드 설정
        levelText.SetText($"레벨 : {equip.level}");
        equipNameText.SetText(equip.name);
        atkText.SetText(tempAtk.ToString("F0"));
        defText.SetText(tempDef.ToString("F0"));
        hpText.SetText(tempHp.ToString("F0"));
        descriptsText.SetText(equip.description);
    }
}
