using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeroManager : MonoBehaviour
{
    public GameObject heroPrefab; // Hero 프리팹을 참조
    public Transform parentTransform; // 생성된 프리팹을 배치할 부모 트랜스폼
    public Transform recruitResultTransform; // 모집 결과를 나타낼 트랜스폼

    private HeroList heroList;
    private List<Hero> ownedHeroes = new List<Hero>();

    public void Initialize(HeroList loadedHeroList)
    {
        heroList = loadedHeroList;

        var sortedHeroes = heroList.heroes.OrderByDescending(h => h.atk).ThenByDescending(h => h.growth).ThenByDescending(h => h.rarity).ThenByDescending(h => h.index).ToList();

        // 영웅 데이터를 불러와서 프리팹을 생성
        foreach (Hero hero in sortedHeroes)
        {
            CreateHeroPrefab(hero, parentTransform);
        }
    }

    void CreateHeroPrefab(Hero hero, Transform parent)
    {
        // 프리팹 인스턴스 생성
        GameObject heroObject = Instantiate(heroPrefab, parent);

        // HeroDisplay 스크립트를 통해 영웅 데이터 설정
        HeroDisplay heroDisplay = heroObject.GetComponent<HeroDisplay>();
        if (heroDisplay != null)
        {
            heroDisplay.SetHeroData(hero);
        }
        else
        {
            Debug.LogError("HeroDisplay component not found on prefab.");
        }
    }

    public void RecruitHero(int times)
    {   
        ClearRecruitResult();
        
        // 랜덤한 확률로 영웅 뽑기
        for (int i = 0; i < times; i++) {
            Hero randomHero = GetRandomHero();
            if (randomHero != null)
            {
                // 모집 결과 트랜스폼에 영웅 노출
                CreateHeroPrefab(randomHero, recruitResultTransform);
            }

            // 보유 영웅 리스트에 추가하고 부모 트랜스폼에 노출
            // ownedHeroes.Add(randomHero);
            // CreateHeroPrefab(randomHero, parentTransform);
        }
    }

    public void ClearRecruitResult()
    {
        foreach (Transform child in recruitResultTransform)
        {
            Destroy(child.gameObject);
        }
    }

    Hero GetRandomHero()
    {
        if (heroList == null || heroList.heroes == null || heroList.heroes.Count == 0)
        {
            Debug.LogError("Hero list is empty or not initialized.");
            return null;
        }

        int randomIndex = Random.Range(0, heroList.heroes.Count);
        return heroList.heroes[randomIndex];
    }
}
