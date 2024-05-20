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
    public GameObject popupHeroDicPrefab;
    private Hero currentHero;

    public void SetHeroData(Hero hero)
    {
        currentHero = hero;

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
    }

    public void OnClick()
    {
        Debug.Log("HeroItem clicked!"); // 디버깅 로그 추가

        // 팝업 생성 및 데이터 설정
        GameObject popup = Instantiate(popupHeroDicPrefab, transform.root); // 부모를 설정 (메인 캔버스 또는 루트)
        popup.transform.SetParent(transform.root, false); // 메인 캔버스의 자식으로 설정
        Debug.Log("Popup instantiated!"); // 팝업 인스턴스화 확인 로그
        PopupHeroDisplay popupHeroDisplay = popup.GetComponent<PopupHeroDisplay>();

        if (popupHeroDisplay != null)
        {
            popupHeroDisplay.SetHeroData(currentHero);
            Debug.Log("Popup data set!"); // 팝업 데이터 설정 확인 로그
        }
        else
        {
            Debug.LogError("PopupHeroDisplay component not found on popup.");
        }
    }
}
