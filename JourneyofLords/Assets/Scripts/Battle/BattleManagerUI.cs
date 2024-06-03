using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManagerUI : MonoBehaviour
{
    public Transform allyGridParent;
    public Transform enemyGridParent;
    public Button skipButton;
    public Button replayButton;
    public GameObject resultPanel;
    public TMP_Text resultText;

    private List<Hero> allies = new List<Hero>();
    private List<Hero> enemies = new List<Hero>();
    private Queue<Hero> turnQueue = new Queue<Hero>();
    private bool battleEnded = false;

    public void InitializeBattle()
    {
        EnsureGridCells(allyGridParent, 9);
        EnsureGridCells(enemyGridParent, 9);

        PlaceHeroes(allyGridParent, GameManager.Instance.SelectedHeroes, allies, GameManager.Instance.allyPrefab);
        PlaceHeroes(enemyGridParent, GameManager.Instance.SelectedStage.enemies.ConvertAll(e => e.hero), enemies, GameManager.Instance.enemyPrefab);

        SetTurnOrder(allies, enemies);

        StartCoroutine(BattleRoutine());

        skipButton.onClick.AddListener(SkipBattle);
        replayButton.onClick.AddListener(ReplayBattle);
        
        // resultPanel 자체에 클릭 이벤트가 아닌, resultPanel 내의 버튼에 클릭 이벤트 추가
        Button closeButton = resultPanel.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseBattleManager);
        }
        else
        {
            Debug.LogError("Close button not found in resultPanel.");
        }
    }

    void EnsureGridCells(Transform parent, int requiredCount)
    {
        int currentCount = parent.childCount;
        for (int i = currentCount; i < requiredCount; i++)
        {
            GameObject emptyCell = Instantiate(GameManager.Instance.emptyCellPrefab, parent);
            emptyCell.name = "EmptyCell";
        }
    }

    void PlaceHeroes(Transform parent, List<Hero> heroes, List<Hero> heroList, GameObject prefab)
    {
        for (int i = 0; i < 9; i++)
        {
            Transform cell = parent.GetChild(i);
            if (i < heroes.Count)
            {
                GameObject heroObject = Instantiate(prefab, cell);
                HeroDisplay heroDisplay = heroObject.GetComponent<HeroDisplay>();

                if (heroDisplay == null)
                {
                    Debug.LogError($"HeroDisplay component is missing on the prefab at index {i}");
                    continue;
                }

                heroDisplay.SetHeroData(heroes[i]);
                heroList.Add(heroes[i]);
            }
            else
            {
                Instantiate(GameManager.Instance.emptyCellPrefab, cell);
            }
        }
    }

    void SetTurnOrder(List<Hero> allies, List<Hero> enemies)
    {
        List<int> allyOrder = new List<int> { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
        List<int> enemyOrder = new List<int> { 0, 3, 6, 1, 4, 7, 2, 5, 8 };

        foreach (int index in allyOrder)
        {
            if (index < allies.Count)
            {
                turnQueue.Enqueue(allies[index]);
            }
        }

        foreach (int index in enemyOrder)
        {
            if (index < enemies.Count)
            {
                turnQueue.Enqueue(enemies[index]);
            }
        }
    }

    IEnumerator BattleRoutine()
    {
        while (!battleEnded)
        {
            if (turnQueue.Count > 0)
            {
                Hero currentHero = turnQueue.Dequeue();
                if (currentHero.IsAlive)
                {
                    Hero target = SelectTarget(currentHero);
                    if (target != null)
                    {
                        yield return StartCoroutine(Attack(currentHero, target));
                        if (AllHeroesDefeated(allies))
                        {
                            EndBattle("Defeat");
                            yield break;
                        }
                        else if (AllHeroesDefeated(enemies))
                        {
                            EndBattle("Victory");
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    Hero SelectTarget(Hero attacker)
    {
        List<Hero> targetList = (allies.Contains(attacker)) ? enemies : allies;
        Transform parent = (allies.Contains(attacker)) ? enemyGridParent : allyGridParent;
        int attackerIndex = FindHeroIndex(attacker, parent);

        int rowStart = (attackerIndex / 3) * 3;
        Hero target = FindFirstAliveHeroInRow(targetList, parent, rowStart);

        if (target == null)
        {
            for (int i = 1; i <= 2; i++)
            {
                int upperRowStart = rowStart - (i * 3);
                int lowerRowStart = rowStart + (i * 3);

                if (upperRowStart >= 0)
                {
                    target = FindFirstAliveHeroInRow(targetList, parent, upperRowStart);
                    if (target != null) break;
                }

                if (lowerRowStart < 9)
                {
                    target = FindFirstAliveHeroInRow(targetList, parent, lowerRowStart);
                    if (target != null) break;
                }
            }
        }

        return target;
    }

    Hero FindFirstAliveHeroInRow(List<Hero> heroList, Transform parent, int rowStart)
    {
        for (int i = 0; i < 3; i++)
        {
            int index = rowStart + i;
            if (index < parent.childCount)
            {
                Hero hero = parent.GetChild(index).GetComponent<HeroDisplay>()?.GetHero();
                if (hero != null && hero.IsAlive)
                {
                    return hero;
                }
            }
        }
        return null;
    }

    int FindHeroIndex(Hero hero, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).GetComponent<HeroDisplay>()?.GetHero() == hero)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator Attack(Hero attacker, Hero target)
    {
        Debug.Log($"{attacker.name} attacks {target.name}");

        float attackerHPPercent = Mathf.Max(attacker.HPPercentage, 0.5f);
        float targetHPPercent = Mathf.Max(target.HPPercentage, 0.5f);

        float damage = Mathf.Max(target.maxLead * 0.01f, (attacker.atk * attackerHPPercent) - (target.def * targetHPPercent));
        target.TakeDamage(damage);

        Debug.Log($"{target.name} takes {damage} damage, remaining HP: {target.lead}");

        yield return new WaitForSeconds(2f);
    }

    bool AllHeroesDefeated(List<Hero> heroes)
    {
        foreach (Hero hero in heroes)
        {
            if (hero.IsAlive)
            {
                return false;
            }
        }
        return true;
    }

    void EndBattle(string result)
    {
        battleEnded = true;
        resultPanel.SetActive(true);
        resultText.text = result;
    }

    void SkipBattle()
    {
        StopAllCoroutines();
        EndBattle("Skipped");
    }

    void ReplayBattle()
    {
        battleEnded = false;
        resultPanel.SetActive(false);
        StartCoroutine(BattleRoutine());
    }

    void CloseBattleManager()
    {
        Destroy(gameObject);
    }
}
