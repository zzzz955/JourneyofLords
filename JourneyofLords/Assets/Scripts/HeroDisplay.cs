using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroDisplay : MonoBehaviour
{
    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText;
    public TMP_Text heroName;
    public TMP_Text heroATK;
    public TMP_Text heroDEF;
    public TMP_Text heroTroops;
    public GameObject popupHeroDicPrefab;
    private Hero currentHero;

    public void SetHeroData(Hero hero)
    {
        currentHero = hero;

        // 스프라이트 설정
        Sprite sprite1 = Resources.Load<Sprite>(hero.spriteName);
        if (sprite1 != null) {spriteImage.sprite = sprite1;}

        Sprite sprite2 = Resources.Load<Sprite>(hero.att);
        if (sprite2 != null) {lvImage.sprite = sprite2;}

        Sprite sprite3 = Resources.Load<Sprite>(hero.grade.ToString());
        if (sprite3 != null) {gradeImage.sprite = sprite3;}

        // 텍스트 필드 설정
        if (levelText != null) {levelText.SetText(hero.level.ToString());}
        if (heroName != null) {heroName.SetText(hero.name);}
        if (heroATK != null) {heroATK.SetText(hero.atk.ToString());}
        if (heroDEF != null) {heroDEF.SetText(hero.def.ToString());}
        if (heroTroops != null) {heroTroops.SetText(hero.lead.ToString());}
    }

    public void OnClick()
    {
        // 팝업 생성 및 데이터 설정
        GameObject popup = Instantiate(popupHeroDicPrefab, transform.root); // 부모를 설정 (메인 캔버스 또는 루트)
        popup.transform.SetParent(transform.root, false); // 메인 캔버스의 자식으로 설정
        PopupHeroDisplay popupHeroDisplay = popup.GetComponent<PopupHeroDisplay>();

        if (popupHeroDisplay != null)
        {
            popupHeroDisplay.SetHeroData(currentHero);
        }
        else
        {
            Debug.LogError("PopupHeroDisplay component not found on popup.");
        }
    }
}
