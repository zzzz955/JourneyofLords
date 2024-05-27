using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleReadyUI : MonoBehaviour
{
    
    public GameObject placeHeroUI;
    public Transform parentTransform;
    public GameObject heroPrefab;

    private Dictionary<string, GameObject> heroPrefabDictionary;
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
}
