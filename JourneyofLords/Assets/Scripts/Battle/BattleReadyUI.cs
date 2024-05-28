using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleReadyUI : MonoBehaviour
{
    
    public GameObject placeHeroUI;
    public Transform parentTransform;
    public GameObject heroPrefab;
    public GameObject allyPrefab;
    public Transform allyGridParent; // 아군 GridLayout의 부모 오브젝트

    private Dictionary<string, GameObject> heroPrefabDictionary;
    private List<Hero> selectedHeroes = new List<Hero>(); // 선택된 영웅 리스트
    private FirestoreManager firestoreManager;

    void Start() {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager not found in the scene.");
        }
    }
    
    public void QuitBattleReadyUI() {
        Destroy(gameObject);
    }

    public async void ShowPlaceHeroUI() {
        HeroList lst = await firestoreManager.GetHeroesData();
        Initialize(lst);
        placeHeroUI.SetActive(true);
        selectedHeroes.Clear();
    }

    public void QuitPlaceHeroUI() {
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
            heroDisplay.OnToggleChanged = OnHeroToggleChanged; // Toggle 변경 콜백 설정
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
            selectedHeroes.Add(hero);
        }
        else
        {
            selectedHeroes.Remove(hero);
        }
    }

    public void PlaceAllies()
    {
        // 아군 GridLayout 초기화
        foreach (Transform child in allyGridParent)
        {
            Destroy(child.gameObject);
        }
        DoPlace(selectedHeroes);
        QuitPlaceHeroUI();
        // 선택된 영웅들을 아군 GridLayout에 배치
    }

    public void DoPlace(List<Hero> selected) {
        foreach (var hero in selected)
        {
            GameObject allyObject = Instantiate(allyPrefab, allyGridParent);
            HeroDisplay heroDisplay = allyObject.GetComponent<HeroDisplay>();
            if (heroDisplay != null)
            {
                heroDisplay.SetHeroData(hero);
            }
        }
    }
}
