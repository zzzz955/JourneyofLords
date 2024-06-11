using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using Unity.VisualScripting;

public class Battle : MonoBehaviour
{
    public Transform allyGroup;
    public Transform enemyGroup;
    public GameObject prefabSlot;
    public GameObject prefabAllyMale;
    public GameObject prefabAllyFemale;
    public GameObject prefabEnemyMale;
    public GameObject prefabEnemyFemale;
    public GameObject prefabEmpty;

    private FirestoreManager firestoreManager;
    private GameManager gameManager;
    private List<GameObject> allyHeroes = new List<GameObject>();
    private List<GameObject> enemyHeroes = new List<GameObject>();
    private List<UnitStats> allyUnits = new List<UnitStats>();
    private List<UnitStats> enemyUnits = new List<UnitStats>();

    // Start is called before the first frame update
    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        gameManager = GameManager.Instance;
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager not found in the scene.");
        }
        StartCoroutine(BattleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAlly(Dictionary<int, Hero> selected)
    {
        // 영웅 객체들 배치
        for (int i = 0; i < 4; i++)
        {
            if (selected[i] != null) {
                if (selected.ContainsKey(i) && selected[i] != null) {
                    GameObject allyHero = Instantiate (prefabSlot, allyGroup);
                    GameObject allyObject = Instantiate (
                        selected[i].sex == "male" ? prefabAllyMale : prefabAllyFemale,
                        new Vector3(-1.8f * i - 2, -1, -1), 
                        Quaternion.identity);
                    HeroDisplay heroDisplay = allyHero.GetComponent<HeroDisplay>();
                    UnitStats unitStats = allyObject.GetComponent<UnitStats>();
                    unitStats.Initialize(selected[i]);

                    if (heroDisplay != null)
                    {
                        heroDisplay.SetHeroData(selected[i]);
                    }
                    allyUnits.Add(unitStats);
                    allyHeroes.Add(allyHero);
                }
            }
            else {
                GameObject emptyCellObject = Instantiate(prefabEmpty, allyGroup);
                allyUnits.Add(null);
            }
        }
    }

    public void CreateEnemy(int currentStageLevel) {
        List<StageData> stageDataList = GameManager.Instance.StageDataList;
        StageData currentStageData = stageDataList.Find(stage => stage.level == currentStageLevel);
        if (currentStageData != null)
        {
            List<Enemy> enemies = currentStageData.enemies;
            for (int i = 0; i < 4; i++) {
                if (i == enemies[i].position) {
                    if (i < enemies.Count && enemies[i] != null) {
                        GameObject enemyHero = Instantiate(prefabSlot, enemyGroup);
                        GameObject enemyObject = Instantiate(
                            enemies[i].hero.sex == "male" ? prefabEnemyMale : prefabEnemyFemale,
                            new Vector3(1.8f * i + 2, -1, -1), 
                            Quaternion.identity);
                        HeroDisplay heroDisplay = enemyHero.GetComponent<HeroDisplay>();
                        UnitStats unitStats = enemyObject.GetComponent<UnitStats>();
                        unitStats.Initialize(enemies[i].hero);
                        if (heroDisplay != null) {
                            heroDisplay.SetHeroData(enemies[i].hero);
                        }
                        enemyUnits.Add(unitStats);
                        enemyHeroes.Add(enemyHero);
                    }
                else {
                        GameObject emptyCellObject = Instantiate(prefabEmpty, enemyGroup);
                        enemyUnits.Add(null);
                    }
                }
            }
        }
    }

    private IEnumerator BattleCoroutine() {
        while (allyUnits.Any(unit => unit != null) && enemyUnits.Any(unit => unit != null)) {
            for (int i = 0; i < 4; i++) {
                if (allyUnits[i] != null) {
                    UnitStats allyUnitStats = allyUnits[i].GetComponent<UnitStats>();
                    for (int j = 0; j < 4; j++) {
                        if (enemyUnits[j] != null) {
                            UnitStats enemyUnitStats = enemyUnits[j].GetComponent<UnitStats>();
                            float damage = allyUnitStats.atk - enemyUnitStats.def;
                            if (damage < 0) {
                                damage = enemyUnitStats.maxHP * 0.01f;
                            }
                            enemyUnitStats.TakeDamage(damage);
                            if (enemyUnitStats.hp <= 0) {
                                Destroy(enemyUnits[j].gameObject);
                                enemyUnits[j] = null;
                            }
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    }
                }

                if (enemyUnits[i] != null) {
                    UnitStats enemyUnitStats = enemyUnits[i].GetComponent<UnitStats>();
                    for (int j = 0; j < 4; j++) {
                        if (allyUnits[j] != null) {
                            UnitStats allyUnitStats = allyUnits[j].GetComponent<UnitStats>();
                            float damage = enemyUnitStats.atk - allyUnitStats.def;
                            if (damage < 0) {
                                damage = allyUnitStats.maxHP * 0.01f;
                            }
                            allyUnitStats.TakeDamage(damage);
                            if (allyUnitStats.hp <= 0) {
                                Destroy(allyUnits[j].gameObject);
                                allyUnits[j] = null;
                            }
                            yield return new WaitForSeconds(1f);
                            break;
                        }
                    }
                }
            }
        }
    }
}
