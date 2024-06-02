using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StageButton : MonoBehaviour
{
    public TMP_Text stageText;

    private int stageNumber;
    private bool isUnlocked;
    private List<Hero> selectedHeroes = new List<Hero>();

    public void Setup(int stage, bool unlocked)
    {
        stageNumber = stage;
        isUnlocked = unlocked;
        stageText.SetText(stage.ToString());
    }

    public void Clicked()
    {
        StageManager stageManager = FindObjectOfType<StageManager>();
        if (stageManager != null)
        {
            stageManager.LoadStage(stageNumber); // 먼저 LoadStage 호출
            stageManager.ShowBattleReadyUI(); // 그 다음 ShowBattleReadyUI 호출
        }
        else
        {
            Debug.LogError("StageManager not found.");
        }
    }
}
