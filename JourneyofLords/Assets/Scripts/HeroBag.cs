using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;
using Unity.VisualScripting;

public class HeroBag : MonoBehaviour
{
    public ExcelReader excelReader;
    public GameObject heroPrefab; // 영웅 정보를 표시할 프리팹
    public Transform gridTransform; // GridLayoutGroup의 Transform
    public TMP_Text cntHeroes;

    private List<Hero> heroes = new List<Hero>();
    private int displayedHeroes = 0;
    private int maxHeroes = 20;

    void Start()
    {
        if (excelReader == null)
        {
            Debug.LogError("ExcelReader is not assigned.");
            return;
        }

        if (heroPrefab == null)
        {
            Debug.LogError("Hero prefab is not assigned.");
            return;
        }

        if (gridTransform == null)
        {
            Debug.LogError("GridTransform is not assigned.");
            return;
        }

        heroes = excelReader.ReadExcel();
        DisplayHeroes();
    }

    void Update() {
        
    }

    void DisplayHeroes()
    {
        for (int i = displayedHeroes; i < heroes.Count; i++)
        {
            GameObject heroObject = Instantiate(heroPrefab, gridTransform);
            Hero hero = heroes[i];
            heroObject.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = hero.Name;
            heroObject.transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = "Level " + hero.Level;
            Sprite heroSprite = LoadSprite(hero.ImagePath);
            displayedHeroes ++;
            if (heroSprite != null)
            {
                heroObject.GetComponent<Image>().sprite = heroSprite;
            }
            else
            {
                Debug.LogError("Failed to load sprite for hero: " + hero.Name + " at path: " + hero.ImagePath);
            }
        }
        UpdateHeroesCnt();
        if (displayedHeroes > 25) {
            AdjustGridLayoutGroup adjustGridLayoutGroup = gridTransform.GetComponent<AdjustGridLayoutGroup>();
            adjustGridLayoutGroup.ModifyGrid(displayedHeroes / 6);
        }
    }

    void DisplayMoreHeroes(int amount)
    {
        maxHeroes += amount;
    }

    void UpdateHeroesCnt() {
        cntHeroes.SetText($"보유 영웅 {displayedHeroes}/{maxHeroes}");
    }

    Sprite LoadSprite(string imagePath)
    {
        // 이미지 경로에서 확장자를 제거
        string resourcePath = imagePath.Replace(".png", "").Replace(".jpg", "");
        Debug.Log("Attempting to load texture from path: " + resourcePath); // 디버깅을 위한 로그

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null)
        {
            // 만약 png로 시도해서 실패했다면 jpg로 다시 시도
            texture = Resources.Load<Texture2D>(resourcePath + ".jpg");
            if (texture == null)
            {
                Debug.LogError("Failed to load texture at path: " + resourcePath);
                return null;
            }
        }

        Debug.Log("Successfully loaded texture: " + resourcePath); // 성공적으로 로드된 경우 로그

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
