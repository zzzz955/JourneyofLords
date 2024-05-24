using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class StageManager : MonoBehaviour
{
    public GameObject stageButtonPrefab; // 스테이지 버튼 프리팹
    public Transform buttonContainer; // 버튼들이 배치될 부모 오브젝트
    public Button nextButton; // 다음 페이지 버튼
    public Button prevButton; // 이전 페이지 버튼

    private int currentPage = 0;
    private int stagesPerPage = 10;
    private int totalStages = 50; // 예시로 총 50개의 스테이지가 있다고 가정
    private int maxStage;

    private FirestoreManager firestoreManager;
    private FirebaseAuth auth;
    private string userId;

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
            stageButton.Setup(i, i <= maxStage + 1);
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
}
