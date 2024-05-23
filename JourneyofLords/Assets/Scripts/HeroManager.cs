using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class HeroManager : MonoBehaviour
{
    public GameObject heroPrefab; 
    public GameObject recruitHeroPrefab; 
    public Transform parentTransform; 
    public Transform parentTransformDic; 
    public Transform recruitResultTransform; 
    public TMP_Text currentHeroesText;
    public GameObject recruitResult;
    public ExtendHeroesbagPopup extendHeroesbagPopup; // ExtendHeroesbagPopup 패널을 참조

    private HeroList heroList;
    private List<List<HeroRate>> heroRatesList;
    private List<Hero> ownedHeroes = new List<Hero>();
    private int currentHeroes;
    private int maxHeroes = 12;

    private FirestoreManager firestoreManager;
    private Dictionary<string, GameObject> heroPrefabDictionary;

    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager not found in the scene.");
        }
        LoadOwnedHeroes();
        Initialize(heroList, false);
    }

    public void Initialize(HeroList list, bool owned)
    {
        heroPrefabDictionary = new Dictionary<string, GameObject>();

        var sortedHeroes = list.heroes.OrderByDescending(h => h.atk).ThenByDescending(h => h.growth).ThenByDescending(h => h.rarity).ThenByDescending(h => h.index).ToList();

        foreach (Hero hero in sortedHeroes)
        {
            if (owned) {
                CreateHeroPrefab(hero, parentTransform, heroPrefab);
            } else {
                CreateHeroPrefab(hero, parentTransformDic, heroPrefab);
            }
        }
        if(owned) {
            currentHeroes = list.heroes.Count;
            UpdateHeroesCnt();
        }
    }

    public void SetHeroRatesList(List<List<HeroRate>> ratesList)
    {
        heroRatesList = ratesList;
    }

    public void SetHeroList(HeroList allHero)
    {
        heroList = allHero;
    }

    void CreateHeroPrefab(Hero hero, Transform parent, GameObject prefab)
    {
        GameObject heroObject = Instantiate(prefab, parent);
        HeroDisplay heroDisplay = heroObject.GetComponent<HeroDisplay>();
        if (heroDisplay != null)
        {
            heroDisplay.SetHeroData(hero);
            heroPrefabDictionary[hero.id] = heroObject;
        }
        else
        {
            Debug.LogError("HeroDisplay component not found on prefab.");
        }
    }

    public void RecruitHero(int tableIndex)
    {   
        int times = (tableIndex == 0 || tableIndex == 1) ? 1 : 10;
        if (currentHeroes + times > maxHeroes) {
            // ExtendHeroesbagPopup 패널을 활성화하고 값을 설정
            extendHeroesbagPopup.Initialize(this);
            extendHeroesbagPopup.CheckVal(times);
            return;
        }

        ClearRecruitResult(recruitResultTransform);
        ClearRecruitResult(parentTransform);
        RecruitResultActive();
        
        for (int i = 0; i < times; i++) {
            Hero randomHero = GetRandomHeroByRate(tableIndex);
            if (randomHero != null)
            {
                randomHero.id = Guid.NewGuid().ToString();
                if (firestoreManager != null)
                {
                    firestoreManager.AddHero(randomHero);
                }
                CreateHeroPrefab(randomHero, recruitResultTransform, recruitHeroPrefab);
            }
            else
            {
                Debug.LogWarning("Failed to recruit hero. GetRandomHeroByRate returned null.");
            }
        }
        LoadOwnedHeroes();
    }

    public void ClearRecruitResult(Transform form)
    {
        foreach (Transform child in form)
        {
            Destroy(child.gameObject);
        }
    }

    Hero GetRandomHeroByRate(int fileIndex)
    {
        if (heroRatesList == null || heroRatesList.Count <= fileIndex || heroRatesList[fileIndex].Count == 0)
        {
            Debug.LogError("Invalid hero rates list.");
            return null;
        }

        float totalRate = heroRatesList[fileIndex].Sum(hr => hr.rate);
        float randomPoint = Random.Range(0f, totalRate);
        float currentRate = 0f;
        foreach (var heroRate in heroRatesList[fileIndex])
        {
            currentRate += heroRate.rate;
            if (randomPoint < currentRate)
            {
                int index = heroRate.index;
                Hero hero = heroList.heroes.FirstOrDefault(h => h.index == index);
                if (hero != null)
                {
                    return hero;
                }
            }
        }
        Debug.LogError("Failed to select a hero based on rates.");
        return null;
    }

    void LoadOwnedHeroes()
    {
        firestoreManager.GetHeroesData();
    }

    public void UpdateHero(Hero updatedHero)
    {
        firestoreManager.UpdateHero(updatedHero);
        Hero hero = heroList.heroes.FirstOrDefault(h => h.id == updatedHero.id);
        if (hero != null)
        {
            hero.level = updatedHero.level;
            hero.atk = updatedHero.atk;
            hero.def = updatedHero.def;
            hero.hp = updatedHero.hp;

            if (heroPrefabDictionary.TryGetValue(updatedHero.id, out GameObject heroObject))
            {
                HeroDisplay heroDisplay = heroObject.GetComponent<HeroDisplay>();
                if (heroDisplay != null)
                {
                    heroDisplay.SetHeroData(hero);
                }
            }
        }
    }

    public void RecruitResultActive () {
        recruitResult.SetActive(true);
    }

    public void RecruitResultQuit () {
        recruitResult.SetActive(false);
    }

    void UpdateHeroesCnt() {
        currentHeroesText.SetText("용량 : " + currentHeroes + " / " + maxHeroes);
    }

    public void Maxpuls(int cnt) {
        maxHeroes += cnt;
        UpdateHeroesCnt();
    }
}
