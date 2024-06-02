using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using TMPro;

public class StageManager : MonoBehaviour
{
    public GameObject stageButtonPrefab; // 스테이지 버튼 프리팹
    public Transform buttonContainer; // 버튼들이 배치될 부모 오브젝트
    public Button nextButton; // 다음 페이지 버튼
    public Button prevButton; // 이전 페이지 버튼
    public TMP_Text energyInfo;
    public Transform parentTransform;
    public GameObject battleReadyUI;

    private int currentPage = 0;
    private int stagesPerPage = 10;
    private int totalStages = 50; // 예시로 총 50개의 스테이지가 있다고 가정
    private int maxStage;

    private FirestoreManager firestoreManager;
    private FirebaseAuth auth;
    private string userId;
    private GameManager gameManager;

    private void Start()
    {
        firestoreManager = FindObjectOfType<FirestoreManager>();
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId;
            nextButton.onClick.AddListener(NextPage);
            prevButton.onClick.AddListener(PrevPage);
            LoadMaxStage();
        }
        else
        {
            Debug.LogError("User is not authenticated.");
        }
        gameManager = GameManager.Instance;
    }

    private void LoadMaxStage()
    {
        firestoreManager.GetUserMaxStage(userId, (maxStage) =>
        {
            this.maxStage = maxStage;
            UpdateButtons();
        });
    }

    public void StageCleared(int stage)
    {
        if (stage == maxStage + 1)
        {
            maxStage++;
            firestoreManager.UpdateUserMaxStage(userId, maxStage);
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        // 기존 버튼 삭제
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // 현재 페이지에 해당하는 버튼 생성
        int startStage = currentPage * stagesPerPage + 1;
        int endStage = Mathf.Min(startStage + stagesPerPage - 1, totalStages);

        for (int i = startStage; i <= endStage; i++)
        {
            GameObject buttonObj = Instantiate(stageButtonPrefab, buttonContainer);
            StageButton stageButton = buttonObj.GetComponent<StageButton>();
            stageButton.Setup(i, i <= maxStage);
        }

        // 페이지 전환 버튼 활성화/비활성화 설정
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = endStage < totalStages;
    }

    private void NextPage()
    {
        currentPage++;
        UpdateButtons();
    }

    private void PrevPage()
    {
        currentPage--;
        UpdateButtons();
    }

    public void UpdateEnergyText(int cur, int max)
    {
        energyInfo.SetText(cur + "/" + max);
    }

    public void ShowBattleReadyUI()
    {
        Debug.Log("ShowBattleReadyUI called");

        if (battleReadyUI == null)
        {
            Debug.LogError("battleReadyUI is not assigned.");
            return;
        }

        if (parentTransform == null)
        {
            Debug.LogError("parentTransform is not assigned.");
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("gameManager is not assigned.");
            return;
        }

        if (gameManager.SelectedHeroes == null)
        {
            Debug.LogError("SelectedHeroes is not assigned.");
            return;
        }

        if (gameManager.SelectedStage == null)
        {
            Debug.LogError("SelectedStage is not assigned.");
            return;
        }

        GameObject instance = Instantiate(battleReadyUI, parentTransform);
        BattleReadyUI battleReadyUIScript = instance.GetComponent<BattleReadyUI>();

        if (battleReadyUIScript != null)
        {
            battleReadyUIScript.DoPlace(gameManager.SelectedHeroes);

            // 적군 배치
            EnemyPlacer enemyPlacer = instance.GetComponentInChildren<EnemyPlacer>();
            if (enemyPlacer != null)
            {
                enemyPlacer.PlaceEnemies(gameManager.SelectedStage.enemies);
            }
            else
            {
                Debug.LogError("EnemyPlacer not found in the BattleReadyUI.");
            }
        }
        else
        {
            Debug.LogError("BattleReadyUI component not found on instantiated object.");
        }
    }

    public void LoadStage(int stageLevel)
    {
        Debug.Log("LoadStage called for stage: " + stageLevel);
        StageData selectedStage = gameManager.StageDataList.Find(s => s.level == stageLevel);
        if (selectedStage != null)
        {
            gameManager.SelectedStage = selectedStage;
        }
        else
        {
            Debug.LogError("Stage data not found for stage level: " + stageLevel);
        }
    }
}
