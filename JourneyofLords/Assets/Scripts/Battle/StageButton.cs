using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StageButton : MonoBehaviour
{
    public TMP_Text stageText;
    public GameObject battleReadyUIPrefab;

    private int stageNumber;
    private bool isUnlocked;
    private List<Hero> selectedHeroes = new List<Hero>();
    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.Instance;
    }

    public void Setup(int stage, bool unlocked)
    {
        stageNumber = stage;
        stageText.SetText(stage.ToString());
        Button button = gameObject.GetComponent<Button>();
        if (button != null && !unlocked)
        {
            // 버튼을 비활성화
            button.interactable = false;
        }
    }

    public void ShowbattleReadyUI() {
        GameObject battleReady = Instantiate(battleReadyUIPrefab, transform.parent.parent);
        BattleReadyUI battleReadyUIScript = battleReady.GetComponent<BattleReadyUI>();
        if (battleReadyUIScript != null) {
            if (gameManager.SelectedHeroes.Count != 0) {
                Dictionary<int, Hero> selected = gameManager.SelectedHeroes;
                if (selected != null) {
                    battleReadyUIScript.DoPlace(selected);
                }
            }
            battleReadyUIScript.DoEnemyPlace(stageNumber);
            battleReadyUIScript.currentStage = stageNumber;
        }
    }
}
