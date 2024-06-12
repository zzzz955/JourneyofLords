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
    public GameObject panelVictory;
    public GameObject panelDefeat;

    private FirestoreManager firestoreManager;
    private GameManager gameManager;
    private List<GameObject> allyHeroes = new List<GameObject>();
    private List<GameObject> enemyHeroes = new List<GameObject>();
    private List<UnitStats> allyUnits = new List<UnitStats>();
    private List<UnitStats> enemyUnits = new List<UnitStats>();

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

    public void CreateAlly(Dictionary<int, Hero> selected)
    {
        for (int i = 0; i < 4; i++)
        {
            if (selected[i] != null)
            {
                if (selected.ContainsKey(i) && selected[i] != null)
                {
                    GameObject allyHero = Instantiate(prefabSlot, allyGroup);
                    GameObject allyObject = Instantiate(
                        selected[i].sex == "male" ? prefabAllyMale : prefabAllyFemale,
                        new Vector3(-1.8f * i - 2, -1, -1),
                        Quaternion.identity
                    );
                    HeroDisplay heroDisplay = allyHero.GetComponent<HeroDisplay>();
                    UnitStats unitStats = allyObject.GetComponent<UnitStats>();
                    Unit unit = allyObject.AddComponent<Unit>();
                    unitStats.Initialize(selected[i]);
                    unit.Initialize(selected[i]); // 영웅의 인덱스에 따라 전략 설정

                    if (heroDisplay != null)
                    {
                        heroDisplay.SetHeroData(selected[i]);
                    }
                    allyUnits.Add(unitStats);
                    allyHeroes.Add(allyHero);
                }
            }
            else
            {
                GameObject emptyCellObject = Instantiate(prefabEmpty, allyGroup);
                allyUnits.Add(null);
            }
        }
    }

    public void CreateEnemy(int currentStageLevel)
    {
        List<StageData> stageDataList = GameManager.Instance.StageDataList;
        StageData currentStageData = stageDataList.Find(stage => stage.level == currentStageLevel);
        if (currentStageData != null)
        {
            List<Enemy> enemies = currentStageData.enemies;
            for (int i = 0; i < 4; i++)
            {
                if (i == enemies[i].position)
                {
                    if (i < enemies.Count && enemies[i] != null)
                    {
                        GameObject enemyHero = Instantiate(prefabSlot, enemyGroup);
                        GameObject enemyObject = Instantiate(
                            enemies[i].hero.sex == "male" ? prefabEnemyMale : prefabEnemyFemale,
                            new Vector3(1.8f * i + 2, -1, -1),
                            Quaternion.identity
                        );
                        HeroDisplay heroDisplay = enemyHero.GetComponent<HeroDisplay>();
                        UnitStats unitStats = enemyObject.GetComponent<UnitStats>();
                        Unit unit = enemyObject.AddComponent<Unit>();
                        unitStats.Initialize(enemies[i].hero);
                        unit.Initialize(enemies[i].hero); // 영웅의 인덱스에 따라 전략 설정

                        if (heroDisplay != null)
                        {
                            heroDisplay.SetHeroData(enemies[i].hero);
                        }
                        enemyUnits.Add(unitStats);
                        enemyHeroes.Add(enemyHero);
                    }
                    else
                    {
                        GameObject emptyCellObject = Instantiate(prefabEmpty, enemyGroup);
                        enemyUnits.Add(null);
                    }
                }
            }
        }
    }

    private IEnumerator BattleCoroutine()
    {
        while (allyUnits.Any(unit => unit != null) && enemyUnits.Any(unit => unit != null))
        {
            for (int i = 0; i < 4; i++)
            {
                if (allyUnits[i] != null)
                {
                    Unit allyUnit = allyUnits[i].GetComponent<Unit>();
                    allyUnit.StartTurn(); // 턴 시작
                    yield return StartCoroutine(PerformAttack(allyUnit, enemyUnits, true)); // 아군이 적군을 공격
                }

                if (enemyUnits[i] != null)
                {
                    Unit enemyUnit = enemyUnits[i].GetComponent<Unit>();
                    enemyUnit.StartTurn(); // 턴 시작
                    yield return StartCoroutine(PerformAttack(enemyUnit, allyUnits, false)); // 적군이 아군을 공격
                }
            }
        }
        yield return new WaitForSeconds(1f);
        if (allyUnits.Any(unit => unit != null))
        {
            panelVictory.SetActive(true);
        }
        else
        {
            panelDefeat.SetActive(true);
        }
        ClearUnits(allyUnits);
        ClearUnits(enemyUnits);

        ResetUnits(allyUnits);
        ResetUnits(enemyUnits);
    }

    private IEnumerator PerformAttack(Unit attacker, List<UnitStats> enemyUnits, bool isAlly)
    {
        List<UnitStats> targets = attacker.TargetStrategy.SelectTargets(enemyUnits.ToArray());

        foreach (UnitStats target in targets)
        {
            if (target != null)
            {
                Vector3 startPos = attacker.transform.position;
                Vector3 endPos;
                if (isAlly)
                {
                    endPos = new Vector3(target.transform.position.x - 1, startPos.y, startPos.z);
                }
                else
                {
                    endPos = new Vector3(target.transform.position.x + 1, startPos.y, startPos.z);
                }
                yield return StartCoroutine(MoveOverTime(attacker.transform, endPos, 0.5f));

                // 개별 공격 수행
                attacker.Attack(new UnitStats[] { target });
                yield return new WaitForSeconds(1f);

                if (target.hp <= 0)
                {
                    Destroy(target.gameObject);
                    // 해당 유닛 리스트에서 null로 설정
                    for (int i = 0; i < enemyUnits.Count; i++)
                    {
                        if (enemyUnits[i] == target)
                        {
                            enemyUnits[i] = null;
                            break;
                        }
                    }
                }

                yield return StartCoroutine(MoveOverTime(attacker.transform, startPos, 0.5f));
            }
        }
    }

    private IEnumerator MoveOverTime(Transform target, Vector3 endPos, float duration)
    {
        Vector3 startPos = target.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.position = endPos;
    }

    private void ClearUnits(List<UnitStats> units)
    {
        foreach (var unit in units)
        {
            if (unit != null)
            {
                Destroy(unit.gameObject);
            }
        }
        units.Clear();
    }

    private void ResetUnits(List<UnitStats> units)
    {
        foreach (var unit in units)
        {
            if (unit != null)
            {
                unit.GetComponent<Unit>().Initialize(unit.hero); // 유닛 상태 재초기화
            }
        }
    }

    public void BattleOut() {
        Destroy(gameObject);
    }
}
