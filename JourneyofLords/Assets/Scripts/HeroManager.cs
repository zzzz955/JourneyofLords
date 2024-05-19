using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public HeroList heroList; // HeroList ScriptableObject를 참조
    public GameObject heroPrefab; // Hero 프리팹을 참조
    public Transform parentTransform; // 생성된 프리팹을 배치할 부모 트랜스폼

    void Start()
    {
        // 영웅 데이터를 불러와서 프리팹을 생성
        foreach (Hero hero in heroList.heroes)
        {
            CreateHeroPrefab(hero);
        }
    }

    void CreateHeroPrefab(Hero hero)
    {
        // 프리팹 인스턴스 생성
        GameObject heroObject = Instantiate(heroPrefab, parentTransform);

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
}
