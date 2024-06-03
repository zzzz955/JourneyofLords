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
}
