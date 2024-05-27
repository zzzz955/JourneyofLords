using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class StageButton : MonoBehaviour
{
    public TMP_Text stageText;


    private int stageNumber;
    private bool isUnlocked;
    private StageManager stageManager;

    public void Setup(int stage, bool unlocked)
    {
        stageNumber = stage;
        isUnlocked = unlocked;
        stageText.SetText(stage.ToString());
    }

    public void Clicked()
    {
        StageManager stageManager = FindObjectOfType<StageManager>();
        if (stageManager != null) {stageManager.ShowBattleReadyUI();}
    }
}
