using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject battlePanel; // 배틀 패널 프리팹
    public GameObject mainPanel; // 메인 패널 프리팹
    public Transform allyGridParent;
    public Transform enemyGridParent;
    public GameObject allyPrefab;
    public GameObject enemyPrefab;

    private GameManager gameManager;
    private List<IUnit> allies = new List<IUnit>();
    private List<IUnit> enemies = new List<IUnit>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스가 있을 경우 파괴
            return;
        }

        gameManager = GameManager.Instance;
    }

    public void ShowBattlePanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (battlePanel != null) battlePanel.SetActive(true);
        InitializeBattle();
    }

    public void HideBattlePanel()
    {
        if (battlePanel != null) battlePanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    void InitializeBattle()
    {
        // 아군 배치
        foreach (Transform child in allyGridParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Hero hero in gameManager.SelectedHeroes)
        {
            GameObject allyObject = Instantiate(allyPrefab, allyGridParent);
            HeroDisplay heroDisplay = allyObject.GetComponent<HeroDisplay>();
            if (heroDisplay != null)
            {
                heroDisplay.SetHeroData(hero);
                allies.Add(heroDisplay);
            }
        }

        // 적군 배치
        foreach (Transform child in enemyGridParent)
        {
            Destroy(child.gameObject);
        }
        StageData currentStage = gameManager.StageDataList.Find(s => s.level == gameManager.CurrentStage);
        if (currentStage != null)
        {
            foreach (Enemy enemy in currentStage.enemies)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, enemyGridParent);
                HeroDisplay heroDisplay = enemyObject.GetComponent<HeroDisplay>();
                if (heroDisplay != null)
                {
                    heroDisplay.SetHeroData(enemy.hero);
                    enemies.Add(heroDisplay);
                }
            }
        }
    }

    public IEnumerator Attack<T>(T attacker, List<T> targets) where T : class, IUnit
    {
        foreach (T target in targets)
        {
            Debug.Log($"{attacker.GetName()}가 {target.GetName()}을(를) 공격합니다");
            ApplyDamage(attacker, target);
            yield return new WaitForSeconds(1f);
        }
    }

    void ApplyDamage(IUnit attacker, IUnit target)
    {
        if (attacker == null || target == null)
        {
            Debug.LogError("Attacker or target is null");
            return;
        }
        target.TakeDamage(attacker.GetAttack());
    }
}
