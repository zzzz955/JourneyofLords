using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using Unity.VisualScripting;
using System.Security;
using TMPro;
using UnityEditor.Search;
using System;

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
    public GameObject panelResult;
    public TMP_Text resultText;
    public Transform resultGroup;
    public GameObject prefabResultSlot;

    public Dictionary<int, Hero> currentStageHeroes;
    public int stageIndex;

    private FirestoreManager firestoreManager;
    private GameManager gameManager;
    private List<GameObject> allyHeroes = new List<GameObject>();
    private List<GameObject> enemyHeroes = new List<GameObject>();
    private List<UnitStats> allyUnits = new List<UnitStats>();
    private List<UnitStats> enemyUnits = new List<UnitStats>();
    private List<UnitStats> tempAllyUnits = new List<UnitStats>();
    private List<UnitStats> tempEnemyUnits = new List<UnitStats>();
    private List<Hero> currentHeroes = new List<Hero>();

    private float atkBouns;
    private float defBouns;
    private float hpBouns;

    void Awake() {
        atkBouns = 1.00f;
        defBouns = 1.00f;
        hpBouns = 1.00f;
    }

    void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        gameManager = GameManager.Instance;
        currentStageHeroes = gameManager.SelectedHeroes;
        bool attBonus = CheckHeroesAtt(currentStageHeroes);
        if (attBonus == true) {
            atkBouns += 0.05f;
            defBouns += 0.05f;
        }
        CreateAlly(currentStageHeroes);
        CreateEnemy(stageIndex);
        StartCoroutine(BattleCoroutine());
    }

    private bool CheckHeroesAtt(Dictionary<int, Hero> selected) {
        // 첫 번째 Hero의 att 값을 가져옴
        if (selected.Values.Any(hero => hero == null)) {
            return false;
        }
        string firstAtt = selected.First().Value.att;

        // 모든 Hero의 att 값이 첫 번째 Hero의 att 값과 동일한지 확인
        return selected.Values.All(hero => hero.att == firstAtt);
    }

    private void CreateAlly(Dictionary<int, Hero> selected)
    {
        for (int i = 0; i < 4; i++)
        {
            if (selected[i] != null)
            {
                if (selected.ContainsKey(i) && selected[i] != null)
                {
                    currentHeroes.Add(selected[i]);
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
                    tempAllyUnits.Add(unitStats);
                    allyHeroes.Add(allyHero);
                }
            }
            else
            {
                GameObject emptyCellObject = Instantiate(prefabEmpty, allyGroup);
                allyUnits.Add(null);
                tempAllyUnits.Add(null);
                allyHeroes.Add(null);
            }
        }
        IncreaseAllyStats(atkBouns, defBouns, hpBouns);
    }

    private void CreateEnemy(int currentStageLevel)
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
                        tempEnemyUnits.Add(unitStats);
                        enemyHeroes.Add(enemyHero);
                    }
                    else
                    {
                        GameObject emptyCellObject = Instantiate(prefabEmpty, enemyGroup);
                        enemyUnits.Add(null);
                        tempEnemyUnits.Add(null);
                        enemyHeroes.Add(null);
                    }
                }
            }
        }
    }

    public void BonusUpdate(float atkPercentage, float defPercentage, float hpPercentage) {
        atkBouns += atkPercentage;
        defBouns += defPercentage;
        hpBouns += hpPercentage;
    }

    public void IncreaseAllyStats(float atkPercentage, float defPercentage, float hpPercentage)
    {
        for (int i = 0; i < allyUnits.Count; i++)
        {
            var allyUnit = allyUnits[i];
            var allyHero = allyHeroes[i];

            if (allyUnit != null)
            {
                allyUnit.IncreaseStats(atkPercentage, defPercentage, hpPercentage);
            }

            if (allyHero != null)
            {
                var heroDisplay = allyHero.GetComponent<HeroDisplay>();
                if (heroDisplay != null)
                {
                    heroDisplay.UpdateStats(allyUnit.atk, allyUnit.def, allyUnit.hp, (atkBouns - 1) * 100, (defBouns - 1) * 100, (hpBouns - 1) * 100);
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
        ShowResult();

        ClearUnits(allyUnits);
        ClearUnits(enemyUnits);

        ResetUnits(tempAllyUnits);
        ResetUnits(tempEnemyUnits);
    }

    private async void ShowResult() {
        panelResult.SetActive(true);
        if (allyUnits.Any(unit => unit != null)) {
            resultText.SetText("Victory!");

            GameObject stageManagerObject = GameObject.Find("StageManager");
            if (stageManagerObject != null) {
                StageManager stageManager = stageManagerObject.GetComponent<StageManager>();
                stageManager.StageCleared(stageIndex);
            }

            int getExp = gameManager.stageEXPList[stageIndex - 1].getEXP;
            List<bool> levelUpResult = await firestoreManager.UpdateHeroEXP(currentHeroes, getExp);
            for (int i = 0; i < currentHeroes.Count; i++) {
                GameObject resultHero = Instantiate(prefabResultSlot, resultGroup);
                HeroDisplay heroDisplay = resultHero.GetComponent<HeroDisplay>();
                if (heroDisplay != null)
                {
                    heroDisplay.isLevelUp = levelUpResult[i];
                    heroDisplay.getEXP = getExp;
                    heroDisplay.SetHeroData(currentHeroes[i]);
                }
            }
        }
        else {
            resultText.SetText("Defeat");
            for (int i = 0; i < currentHeroes.Count; i++) {
                GameObject resultHero = Instantiate(prefabResultSlot, resultGroup);
                HeroDisplay heroDisplay = resultHero.GetComponent<HeroDisplay>();
                if (heroDisplay != null)
                {
                    heroDisplay.SetHeroData(currentHeroes[i]);
                }
            }
        }
    }

    private IEnumerator PerformAttack(Unit attacker, List<UnitStats> enemyUnits, bool isAlly)
    {
        Vector3 startPos = attacker.transform.position;
        for (int i = 0; i < attacker.initialAttackCount; i++) {
            List<UnitStats> targets = attacker.TargetStrategy.SelectTargets(enemyUnits.ToArray());
            if (targets.Count > 0)
            {
                // Move to the first target's position as a representative move
                Vector3 endPos = isAlly 
                    ? new Vector3(targets[0].transform.position.x - 1, startPos.y, startPos.z)
                    : new Vector3(targets[0].transform.position.x + 1, startPos.y, startPos.z);
                yield return StartCoroutine(MoveOverTime(attacker.transform, endPos, 0.5f));

                // Perform attack on all targets
                attacker.Attack(targets.ToArray());
                yield return new WaitForSeconds(1f);

                foreach (UnitStats target in targets)
                {
                    if (target.hp <= 0)
                    {
                        Destroy(target.gameObject);
                        // Remove the target from the enemyUnits list
                        for (int j = 0; j < enemyUnits.Count; j++)
                        {
                            if (enemyUnits[j] == target)
                            {
                                enemyUnits[j] = null;
                                break;
                            }
                        }
                    }
                }
            }
        }
        yield return StartCoroutine(MoveOverTime(attacker.transform, startPos, 0.5f));
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
