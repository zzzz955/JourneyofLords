using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class BattleReadyUI : MonoBehaviour
{
    public GameObject placeHeroUI;
    public Transform parentTransform;
    public GameObject heroPrefab;
    public GameObject allyPrefab;
    public GameObject emptyCellPrefab;
    public Transform allyGridParent;
    public Button startBattleButton; // 전투 시작 버튼
    public GameObject battleUIPrefab; // 전투 UI 프리팹

    private Dictionary<string, GameObject> heroPrefabDictionary;
    private FirestoreManager firestoreManager;
    private GameManager gameManager;

    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        gameManager = GameManager.Instance;
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager not found in the scene.");
        }

        startBattleButton.onClick.AddListener(OnStartBattleButtonClicked); // 전투 시작 버튼 클릭 리스너 추가
    }

    public void QuitBattleReadyUI()
    {
        Destroy(gameObject);
    }

    public async void ShowPlaceHeroUI()
    {
        HeroList lst = await firestoreManager.GetHeroesData();
        Initialize(lst);
        placeHeroUI.SetActive(true);
        gameManager.SelectedHeroes.Clear();
    }

    public void QuitPlaceHeroUI()
    {
        ClearRecruitResult(parentTransform);
        placeHeroUI.SetActive(false);
    }

    public void Initialize(HeroList list)
    {
        if (list == null)
        {
            Debug.LogError("HeroList is null in Initialize");
            return;
        }
        heroPrefabDictionary = new Dictionary<string, GameObject>();

        var sortedHeroes = list.heroes.OrderByDescending(h => h.atk).ThenByDescending(h => h.growth).ThenByDescending(h => h.rarity).ThenByDescending(h => h.index).ToList();

        foreach (Hero hero in sortedHeroes)
        {
            CreateHeroPrefab(hero, parentTransform, heroPrefab);
        }
    }

    void CreateHeroPrefab(Hero hero, Transform parent, GameObject prefab)
    {
        GameObject heroObject = Instantiate(prefab, parent);
        HeroDisplay heroDisplay = heroObject.GetComponent<HeroDisplay>();

        if (heroDisplay != null)
        {
            heroDisplay.SetHeroData(hero);
            heroDisplay.OnToggleChanged = OnHeroToggleChanged;
            heroPrefabDictionary[hero.id] = heroObject;
        }
        else
        {
            Debug.LogError("HeroDisplay component not found on prefab.");
        }
    }

    public void ClearRecruitResult(Transform form)
    {
        foreach (Transform child in form)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnHeroToggleChanged(Hero hero, bool isOn)
    {
        if (isOn)
        {
            if (gameManager.SelectedHeroes.Count >= 4)
            {
                var toggle = heroPrefabDictionary[hero.id].GetComponent<HeroDisplay>().selectToggle;
                toggle.isOn = false;
                GameManager.Instance.ShowSystemMessage("4명 이상의 영웅을 선택할 수 없습니다.");
            }
            else
            {
                gameManager.SelectedHeroes.Add(hero);
            }
        }
        else
        {
            gameManager.SelectedHeroes.Remove(hero);
        }
    }

    public void PlaceAllies()
    {
        foreach (Transform child in allyGridParent)
        {
            Destroy(child.gameObject);
        }
        DoPlace(gameManager.SelectedHeroes);
        QuitPlaceHeroUI();
    }

    public void DoPlace(List<Hero> selected)
    {
        int gridSize = 3 * 3; // 3x3 그리드
        int totalHeroes = selected.Count;

        // 영웅 객체들 배치
        for (int i = 0; i < totalHeroes; i++)
        {
            GameObject allyObject = Instantiate(allyPrefab, allyGridParent);
            HeroDisplay heroDisplay = allyObject.GetComponent<HeroDisplay>();
            ClickableHero clickableHero = allyObject.GetComponent<ClickableHero>();
            DropZone dropZone = allyObject.GetComponent<DropZone>();

            if (heroDisplay != null)
            {
                heroDisplay.SetHeroData(selected[i]);
            }

            if (clickableHero != null)
            {
                clickableHero.heroData = selected[i];
            }
        }

        // 빈 셀로 나머지 그리드 채우기
        for (int i = totalHeroes; i < gridSize; i++)
        {
            GameObject emptyCellObject = Instantiate(emptyCellPrefab, allyGridParent);
            DropZone dropZone = emptyCellObject.GetComponent<DropZone>();
            ClickableHero clickableHero = emptyCellObject.GetComponent<ClickableHero>();
        }
    }

    // 전투 시작 버튼 클릭 핸들러
    private void OnStartBattleButtonClicked()
    {
        // 선택된 영웅이 1명 이상이어야 전투 시작
        if (gameManager.SelectedHeroes.Count > 0)
        {
            ShowBattleUI();
        }
        else
        {
            GameManager.Instance.ShowSystemMessage("전투를 시작하려면 최소 1명의 영웅을 선택해야 합니다.");
        }
    }

    private void ShowBattleUI()
    {
        // Main Canvas를 명확하게 찾습니다.
        Canvas mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("Main Canvas not found in the scene.");
            return;
        }

        // BattleUI 프리팹을 mainCanvas의 자식으로 인스턴스화합니다.
        GameObject battleUI = Instantiate(battleUIPrefab, mainCanvas.transform);
        battleUI.SetActive(true);

        // BattleManagerUI 컴포넌트를 가져와서 초기화합니다.
        BattleManagerUI battleManager = battleUI.GetComponentInChildren<BattleManagerUI>();

        if (battleManager != null)
        {
            battleManager.InitializeBattle();
        }
        else
        {
            Debug.LogError("BattleManagerUI 컴포넌트를 찾을 수 없습니다.");
            Destroy(battleUI);
        }
    }
}
