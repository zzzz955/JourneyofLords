using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;


public class PopupHeroDisplay : MonoBehaviour
{
    public GameObject panel;
    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText1;
    public TMP_Text levelText2;
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

    void Start () {
        if (panel != null)
        {
            EventTrigger trigger = panel.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = panel.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnCloseButtonClick(); });
            trigger.triggers.Add(entry);
        }
    }

    public void SetHeroData(Hero hero, float tempAtk, float tempDef, float tempHp)
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
        levelText1.SetText($"레벨 : {hero.level}");
        levelText2.SetText(hero.level.ToString());
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
        atkText.SetText("공격력 : " + tempAtk.ToString("F0"));
        defText.SetText("방어력 : " + tempDef.ToString("F0"));
        hpText.SetText("체력 : " + tempHp.ToString("F0"));
        etcText.SetText(hero.description);
    }
    
    void OnCloseButtonClick()
    {
        // 팝업 닫기
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        Destroy(parentCanvas.gameObject);
    }
}
