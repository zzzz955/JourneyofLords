using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeroManager : MonoBehaviour
{
    public GameObject heroPrefab; // Hero 프리팹을 참조
    public GameObject recruitHeroPrefab; // Hero 프리팹을 참조
    public Transform parentTransform; // 생성된 프리팹을 배치할 부모 트랜스폼
    public Transform parentTransformDic; // 생성된 프리팹을 배치할 부모 트랜스폼
    public Transform recruitResultTransform; // 모집 결과를 나타낼 트랜스폼

    private HeroList heroList;
    private List<List<HeroRate>> heroRatesList;
    private List<Hero> ownedHeroes = new List<Hero>();

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

        // 영웅 데이터를 불러와서 프리팹을 생성
        foreach (Hero hero in sortedHeroes)
        {
            if (owned) {
                CreateHeroPrefab(hero, parentTransform, heroPrefab);
            } else {
                CreateHeroPrefab(hero, parentTransformDic, heroPrefab);
            }
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
        // 프리팹 인스턴스 생성
        GameObject heroObject = Instantiate(prefab, parent);

        // HeroDisplay 스크립트를 통해 영웅 데이터 설정
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
        ClearRecruitResult(recruitResultTransform);
        ClearRecruitResult(parentTransform);

        int times = (tableIndex == 0 || tableIndex == 1) ? 1 : 10;
        
        // 랜덤한 확률로 영웅 뽑기
        for (int i = 0; i < times; i++) {
            Hero randomHero = GetRandomHeroByRate(tableIndex);
            if (randomHero != null)
            {
                // 고유한 ID 생성
                randomHero.id = Guid.NewGuid().ToString();
                Debug.Log(randomHero.id);
                Debug.Log((firestoreManager == null) ? "null":"not null");

                // Firebase에 영웅 추가
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
        if (heroRatesList == null)
        {
            Debug.LogError("Hero rates list is null.");
            return null;
        }

        if (heroRatesList.Count <= fileIndex)
        {
            Debug.LogError($"Invalid file index: {fileIndex}. heroRatesList.Count: {heroRatesList.Count}");
            return null;
        }

        if (heroRatesList[fileIndex].Count == 0)
        {
            Debug.LogError($"Hero rates list for the given index {fileIndex} is empty.");
            return null;
        }

        float totalRate = heroRatesList[fileIndex].Sum(hr => hr.rate);
        if (totalRate <= 0)
        {
            Debug.LogError($"Total rate is zero or negative for index {fileIndex}. Total rate: {totalRate}");
            return null;
        }

        float randomPoint = Random.Range(0f, totalRate);
        Debug.Log($"Random Point: {randomPoint} / Total Rate: {totalRate}");

        float currentRate = 0f;
        foreach (var heroRate in heroRatesList[fileIndex])
        {
            currentRate += heroRate.rate;
            if (randomPoint < currentRate)
            {
                int index = heroRate.index;
                Hero hero = heroList.heroes.FirstOrDefault(hero => hero.index == index);
                if (hero != null)
                {
                    Debug.Log($"Selected Hero Index: {index}");
                    return hero;
                }
                else
                {
                    Debug.LogError($"Hero not found for index: {index}. Make sure heroList contains heroes with all necessary indices.");
                    return null;
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
        // Firebase 업데이트
        firestoreManager.UpdateHero(updatedHero);

        // 로컬 데이터 업데이트
        Hero hero = heroList.heroes.FirstOrDefault(h => h.id == updatedHero.id);
        if (hero != null)
        {
            hero.level = updatedHero.level;
            hero.atk = updatedHero.atk;
            hero.def = updatedHero.def;
            hero.hp = updatedHero.hp;
            // 필요한 경우 다른 속성들도 업데이트

            // 프리팹 업데이트
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
}