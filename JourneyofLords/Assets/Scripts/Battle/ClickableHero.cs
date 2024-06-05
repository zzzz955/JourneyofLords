using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableHero : MonoBehaviour, IPointerClickHandler
{
    public Hero heroData;
    private static GameObject selectedHero;
    private Outline outline;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.yellow;
        outline.effectDistance = new Vector2(5, 5);
        outline.enabled = false;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedHero == null)
        {
            if (heroData != null)
            {
                // 첫 번째 영웅 선택
                selectedHero = gameObject;
                outline.enabled = true;
            }
        }
        else if (selectedHero == gameObject)
        {
            // 선택 취소
            outline.enabled = false;
            selectedHero = null;
        }
        else
        {
            // 선택된 영웅과 위치 교환
            SwapPositions(selectedHero, gameObject);
            selectedHero.GetComponent<Outline>().enabled = false;
            selectedHero = null;
        }
    }

    private void SwapPositions(GameObject hero1, GameObject hero2)
    {
        Transform parent1 = hero1.transform.parent;
        Transform parent2 = hero2.transform.parent;

        int siblingIndex1 = hero1.transform.GetSiblingIndex();
        int siblingIndex2 = hero2.transform.GetSiblingIndex();

    // obj1의 Hero 정보를 가져옴
        HeroDisplay heroDisplay1 = hero1.GetComponent<HeroDisplay>();
        Hero heroData1 = heroDisplay1 != null ? heroDisplay1.currentHero : null;

        // obj2의 Hero 정보를 가져옴
        HeroDisplay heroDisplay2 = hero2.GetComponent<HeroDisplay>();
        Hero heroData2 = heroDisplay2 != null ? heroDisplay2.currentHero : null;

        hero1.transform.SetParent(parent2);
        hero1.transform.SetSiblingIndex(siblingIndex2);

        hero2.transform.SetParent(parent1);
        hero2.transform.SetSiblingIndex(siblingIndex1);

        var hero1Key = heroData1 != null ? gameManager.SelectedHeroes.FirstOrDefault(x => x.Value == heroData1).Key : default(int?);
        var hero2Key = heroData2 != null ? gameManager.SelectedHeroes.FirstOrDefault(x => x.Value == heroData2).Key : default(int?);

        if (gameManager.SelectedHeroes.ContainsKey(siblingIndex1) || gameManager.SelectedHeroes.ContainsKey(siblingIndex2))
        {
            // 임시 변수에 값을 저장
            var temp = gameManager.SelectedHeroes.ContainsKey(siblingIndex1) ? gameManager.SelectedHeroes[siblingIndex1] : null;

            if (gameManager.SelectedHeroes.ContainsKey(siblingIndex2))
            {
                gameManager.SelectedHeroes[siblingIndex1] = gameManager.SelectedHeroes[siblingIndex2];
            }
            else
            {
                gameManager.SelectedHeroes.Remove(siblingIndex1);
            }

            if (temp != null)
            {
                gameManager.SelectedHeroes[siblingIndex2] = temp;
            }
            else
            {
                gameManager.SelectedHeroes.Remove(siblingIndex2);
            }
        }
        
        foreach (var kvp in gameManager.SelectedHeroes) {
            int key = kvp.Key;
            Hero val = kvp.Value;

            if (val != null)
            {
                Debug.Log($"Key: {key}, Hero: {val.name}");
            }
            else
            {
                Debug.Log($"Key: {key}, Hero is null");
            }
        }
    }
}