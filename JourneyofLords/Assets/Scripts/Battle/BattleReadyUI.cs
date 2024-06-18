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
    public GameObject enemyPrefab;
    public GameObject emptyCellPrefab;
    public Transform allyGridParent;
    public Transform enemyGridParent;
    public Button startBattleButton; // 전투 시작 버튼
    public GameObject battleUIPrefab; // 전투 UI 프리팹

    private Dictionary<string, GameObject> heroPrefabDictionary;
    private Dictionary<int, Hero> tempSelectedHeroes = new Dictionary<int, Hero>();
    private FirestoreManager firestoreManager;
    private GameManager gameManager;
    public int currentStage;

    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        gameManager = GameManager.Instance;
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager not found in the scene.");
        }
    }

    public void QuitBattleReadyUI()
    {
        Destroy(gameObject);
    }

    public async void ShowPlaceHeroUI()
    {
        tempSelectedHeroes.Clear();
        HeroList lst = await firestoreManager.GetHeroesData();
        Initialize(lst);

        // gameManager.SelectedHeroes에 있는 영웅들의 selectToggle을 on으로 설정
        if (gameManager.SelectedHeroes != null && gameManager.SelectedHeroes.Count > 0)
        {
            foreach (var selectedHero in gameManager.SelectedHeroes.Values)
            {
                if (selectedHero != null && heroPrefabDictionary.ContainsKey(selectedHero.id))
                {
                    var heroDisplay = heroPrefabDictionary[selectedHero.id].GetComponent<HeroDisplay>();
                    if (heroDisplay != null)
                    {
                        heroDisplay.CheckAndToggleHero(gameManager); // CheckAndToggleHero 호출
                    }
                }
            }
        }

        placeHeroUI.SetActive(true);
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
            if (tempSelectedHeroes.Count >= 4)
            {
                var toggle = heroPrefabDictionary[hero.id].GetComponent<HeroDisplay>().selectToggle;
                toggle.isOn = false;
                GameManager.Instance.ShowSystemMessage("4명 이상의 영웅을 선택할 수 없습니다.");
            }
            else
            {
                int index = tempSelectedHeroes.Count;
                tempSelectedHeroes.Add(index, hero);
            }
        }
        else
        {
            var entry = tempSelectedHeroes.FirstOrDefault(x => x.Value.id == hero.id);
                    if (!entry.Equals(default(KeyValuePair<int, Hero>)))
                    {
                        tempSelectedHeroes.Remove(entry.Key);
                        // 키 값을 다시 0, 1, 2, 3으로 재정렬
                        var newSelectedHeroes = tempSelectedHeroes.Values.ToList();
                        tempSelectedHeroes.Clear();
                        for (int i = 0; i < newSelectedHeroes.Count; i++)
                        {
                            tempSelectedHeroes.Add(i, newSelectedHeroes[i]);
                        }
                    }
        }
    }

    public void PlaceAllies()
    {
        if (tempSelectedHeroes.Count < 4) {
            for (int i = tempSelectedHeroes.Count; i < 4; i++) {
                tempSelectedHeroes.Add(i, null);
            }
        }
        foreach (Transform child in allyGridParent)
        {
            Destroy(child.gameObject);
        }
        gameManager.SelectedHeroes = new Dictionary<int, Hero>(tempSelectedHeroes);
        DoPlace(gameManager.SelectedHeroes);
        QuitPlaceHeroUI();
    }

    public void DoPlace(Dictionary<int, Hero> selected)
    {
        // 영웅 객체들 배치
        for (int i = 0; i < 4; i++)
        {
            if (selected[i] != null) {
                GameObject allyObject = Instantiate(allyPrefab, allyGridParent);
                HeroDisplay heroDisplay = allyObject.GetComponent<HeroDisplay>();
                ClickableHero clickableHero = allyObject.AddComponent<ClickableHero>();
                DropZone dropZone = allyObject.AddComponent<DropZone>();

                if (heroDisplay != null)
                {
                    heroDisplay.SetHeroData(selected[i]);
                }

                if (clickableHero != null)
                {
                    clickableHero.heroData = selected[i];
                }
            }
            else {
                GameObject emptyCellObject = Instantiate(emptyCellPrefab, allyGridParent);
                DropZone dropZone = emptyCellObject.AddComponent<DropZone>();
                ClickableHero clickableHero = emptyCellObject.AddComponent<ClickableHero>();
            }
        }
    }

    public void DoEnemyPlace(int currentStageLevel) {
        List<StageData> stageDataList = GameManager.Instance.StageDataList;
        StageData currentStageData = stageDataList.Find(stage => stage.level == currentStageLevel);
        if (currentStageData != null)
        {
            List<Enemy> enemies = currentStageData.enemies;
            for (int i = 0; i < 4; i++) {
                if (i == enemies[i].position) {
                    GameObject enemyObject = Instantiate(enemyPrefab, enemyGridParent);
                    HeroDisplay heroDisplay = enemyObject.GetComponent<HeroDisplay>();
                    if (heroDisplay != null) {
                        heroDisplay.SetHeroData(enemies[i].hero);
                    }
                else {
                        GameObject emptyCellObject = Instantiate(emptyCellPrefab, enemyGridParent);
                    }
                }
            }
        }
    }

    public void StartBattle() {
        GameObject battleIn = Instantiate(battleUIPrefab, transform.parent);
        Battle battle = battleIn.GetComponent<Battle>();
        battle.stageIndex = currentStage;
        Destroy(gameObject);
    }
}