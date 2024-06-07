using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class HeroDisplay : MonoBehaviour
{
    public Image spriteImage;
    public Image lvImage;
    public Image gradeImage;
    public TMP_Text levelText;
    public TMP_Text heroName;
    public TMP_Text heroATK;
    public TMP_Text heroDEF;
    public TMP_Text heroHP;
    public GameObject popupHeroDicPrefab;
    public Toggle selectToggle; // 체크박스 추가

    public Hero currentHero { get; private set; }
    public System.Action<Hero, bool> OnToggleChanged; // 체크박스 변경 콜백

    public void SetHeroData(Hero hero)
    {
        currentHero = hero;

        // 스프라이트 설정
        if (spriteImage != null) {
            Sprite sprite1 = Resources.Load<Sprite>(hero.spriteName);
            if (sprite1 != null) {spriteImage.sprite = sprite1;}
        }

        if (lvImage != null) {
            Sprite sprite2 = Resources.Load<Sprite>(hero.att);
            if (sprite2 != null) {lvImage.sprite = sprite2;}
        }

        if (gradeImage != null) {
            Sprite sprite3 = Resources.Load<Sprite>(hero.grade.ToString());
            if (sprite3 != null) {gradeImage.sprite = sprite3;}
        }

        // 텍스트 필드 설정
        if (levelText != null) {levelText.SetText(hero.level.ToString());}
        if (heroName != null) {heroName.SetText(hero.name);}
        if (heroATK != null) {heroATK.SetText(hero.atk.ToString("F0"));}
        if (heroDEF != null) {heroDEF.SetText(hero.def.ToString("F0"));}
        if (heroHP != null) {heroHP.SetText(hero.hp.ToString("F0"));}

        // 체크박스 설정
        if (selectToggle != null) {selectToggle.onValueChanged.AddListener(OnToggleValueChanged);}
    }

    public Hero GetHero()
    {
        return currentHero;
    }

    public void OnClick()
    {
        // 팝업 생성 및 데이터 설정
        GameObject newCanvasObject = new GameObject("NewCanvas");
        Canvas newCanvas = newCanvasObject.AddComponent<Canvas>();
        newCanvas.renderMode = RenderMode.ScreenSpaceOverlay; // 필요한 경우 다른 Render Mode로 설정
        newCanvasObject.AddComponent<CanvasScaler>();
        newCanvasObject.AddComponent<GraphicRaycaster>();

        // 새로운 Canvas의 RectTransform 설정
        RectTransform canvasRectTransform = newCanvasObject.GetComponent<RectTransform>();
        canvasRectTransform.sizeDelta = new Vector2(1920, 1080); // 원하는 크기로 설정

        CanvasScaler canvasScaler = newCanvasObject.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0.5f; // 원하는 스크린 맞춤 비율 설정

        GameObject popup = Instantiate(popupHeroDicPrefab, newCanvasObject.transform); // 부모를 설정 (메인 캔버스 또는 루트)
        // popup.transform.SetParent(transform.root, false); // 메인 캔버스의 자식으로 설정
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

    private void OnToggleValueChanged(bool isOn)
    {
        OnToggleChanged?.Invoke(currentHero, isOn);
    }

    public void CheckAndToggleHero(GameManager gameManager)
    {
        if (currentHero == null)
        {
            Debug.LogWarning("currentHero is null.");
            return;
        }

        if (gameManager == null || gameManager.SelectedHeroes == null)
        {
            Debug.LogWarning("gameManager or gameManager.SelectedHeroes is null.");
            return;
        }

        bool heroExists = gameManager.SelectedHeroes.Any(x => x.Value != null && x.Value.id == currentHero.id);

        if (heroExists)
        {
            // selectToggle을 켜서 OnToggleChanged를 호출
            if (selectToggle != null && !selectToggle.isOn)
            {
                selectToggle.isOn = true;
            }
        }
        else
        {
            // selectToggle을 꺼서 OnToggleChanged를 호출
            if (selectToggle != null && selectToggle.isOn)
            {
                selectToggle.isOn = false;
            }
        }
    }
}
