using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        GetComponent<Button>().interactable = unlocked;

        if (unlocked)
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        Debug.Log("Stage " + stageNumber + " clicked!");
        // 여기에 스테이지 시작 로직 추가
        stageManager = FindObjectOfType<StageManager>();
        stageManager.StageCleared(stageNumber);
    }
}
