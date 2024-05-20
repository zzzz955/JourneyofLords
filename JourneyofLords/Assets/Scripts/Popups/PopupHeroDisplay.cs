using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupHeroDisplay : MonoBehaviour
{
    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText;
    public TMP_Text heroNameText;
    public TMP_Text gradeText;
    public TMP_Text attText;
    public TMP_Text sexText;
    public TMP_Text growthText;
    public TMP_Text atkText;
    public TMP_Text defText;
    public TMP_Text hpText;
    public TMP_Text etcText;
    public TMP_Text attMoreInfoText;


    public void SetHeroData(Hero hero)
    {
        // 스프라이트 설정
        Sprite sprite1 = Resources.Load<Sprite>(hero.spriteName);
        if (sprite1 != null)
        {
            spriteImage.sprite = sprite1;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.spriteName);
        }

        Sprite sprite2 = Resources.Load<Sprite>(hero.att);
        if (sprite2 != null)
        {
            lvImage.sprite = sprite2;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.att);
        }

        Sprite sprite3 = Resources.Load<Sprite>(hero.grade.ToString());
        if (sprite3 != null)
        {
            gradeImage.sprite = sprite3;
        }
        else
        {
            Debug.LogError("Failed to load sprite: " + hero.grade.ToString());
        }

        // 텍스트 필드 설정
        levelText.SetText(hero.level.ToString());
        heroNameText.SetText(hero.name);
        gradeText.SetText("등급 : " + hero.grade + "성 영웅");
        if (hero.att == "red") {
            attText.SetText("속성 : 불");
            attMoreInfoText.SetText("불의 속성");
        }
        if (hero.att == "blue") {
            attText.SetText("속성 : 물");
            attMoreInfoText.SetText("물의 속성");
        }
        if (hero.att == "green") {
            attText.SetText("속성 : 땅");
            attMoreInfoText.SetText("땅의 속성");
        }
        if (hero.sex == "male") sexText.SetText("성별 : 남자");
        if (hero.sex == "female") sexText.SetText("성별 : 여자");
        growthText.SetText("성장 : " + hero.growth);
        atkText.SetText("공격력 : " + hero.atk);
        defText.SetText("방어력 : " + hero.def);
        hpText.SetText("체력 : " + hero.hp);
        etcText.SetText(hero.description);
    }

    public void Quit() {
        Destroy(gameObject);
    }
}