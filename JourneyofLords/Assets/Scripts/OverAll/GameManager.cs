using System;
using System.Collections;
using System.Collections.Generic; // Ensure this is included
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public string heroDataFilePath = "Scripts/GameData/HeroData.xlsx";
    public string[] rateDataFilePaths = { "Scripts/GameData/HeroRecruit1.xlsx", "Scripts/GameData/HeroRecruit2.xlsx", "Scripts/GameData/HeroRecruit3.xlsx" };
    public string stageDataFilePath = "Scripts/GameData/StageData.xlsx";
    public string levelDataFilePath = "Scripts/GameData/levelData.xlsx";
    public string stageEXPFilePath = "Scripts/GameData/stageEXP.xlsx";

    public HeroList HeroList { get; private set; }
    public List<List<HeroRate>> HeroRatesList { get; private set; }
    public List<StageData> StageDataList { get; private set; }
    public List<LevelData> levelDataList { get; private set; }
    public List<StageEXP> stageEXPList { get; private set; }
    public User CurrentUser { get; private set; }
    public MainUI MainUI { get; private set; }
    public HeroManager HeroManager { get; private set; }
    public Dictionary<int, Hero> SelectedHeroes { get; set; } = new Dictionary<int, Hero>();
    public StageData SelectedStage { get; set; }

    private FirestoreManager firestoreManager;

    // 시스템 메시지 관련 필드 추가
    public TMP_Text systemMessageTextPrefab;
    public Transform systemMessageParent; // 메시지를 부모로 설정할 Transform
    private float displayDuration = 3f;
    private List<GameObject> activeMessages = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager Awake 호출됨");
            firestoreManager = FindObjectOfType<FirestoreManager>();
            LoadAllData();            
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            MainUI = FindObjectOfType<MainUI>();
            if (MainUI != null && CurrentUser != null)
            {
                MainUI.UpdatePlayerStatus(CurrentUser);
            }
        }
    }

    public void OnHeroesLoaded()
    {
        HeroManager heroManager = FindObjectOfType<HeroManager>();
        if (heroManager != null)
        {
            if (CurrentUser != null)
            {
                heroManager.MaxInitialize(CurrentUser.maxHeroes);
            }
            heroManager.SetHeroList(HeroList);
            heroManager.SetHeroRatesList(HeroRatesList);
        }
    }

    private void LoadAllData()
    {
        ExcelDataLoader dataLoader = ScriptableObject.CreateInstance<ExcelDataLoader>();
        HeroList = dataLoader.LoadHeroData(Path.Combine(Application.dataPath, heroDataFilePath));
        HeroRatesList = new List<List<HeroRate>>();
        foreach (string rateDataFilePath in rateDataFilePaths)
        {
            HeroRatesList.Add(dataLoader.LoadRateData(Path.Combine(Application.dataPath, rateDataFilePath)));
        }
        StageDataList = dataLoader.LoadStageData(Path.Combine(Application.dataPath, stageDataFilePath));
        levelDataList = dataLoader.LoadLevelData(Path.Combine(Application.dataPath, levelDataFilePath));
        stageEXPList = dataLoader.LoadStageEXP(Path.Combine(Application.dataPath, stageEXPFilePath));

        Debug.Log($"Loaded {HeroList.heroes.Count} heroes.");
        Debug.Log($"Loaded {HeroRatesList.Count} heroRates.");
        Debug.Log($"Loaded {StageDataList.Count} stages.");
        Debug.Log($"Loaded {levelDataList.Count} levels.");
        Debug.Log($"Loaded {stageEXPList.Count} stageEXPs.");
    }

    public void SetUserData(User userData)
    {
        Debug.Log("SetUserData called with user: " + userData.userID);
        CurrentUser = userData;
    }

    private void SaveUserData()
    {
        firestoreManager.UpdateUserData(CurrentUser, 
            onSuccess: () => Debug.Log("User data updated successfully."),
            onFailure: (error) => Debug.LogError("Error updating user data: " + error));
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GetEnergyInfo() {
        
    }

    // 시스템 메시지 표시 메서드 수정
    public void ShowSystemMessage(string message)
    {
        GameObject messageObject = Instantiate(systemMessageTextPrefab.gameObject, systemMessageParent);
        TMP_Text messageText = messageObject.GetComponent<TMP_Text>();
        messageText.text = message;
        activeMessages.Add(messageObject);

        // 기존 메시지들을 위로 이동
        for (int i = 0; i < activeMessages.Count - 1; i++)
        {
            RectTransform rectTransform = activeMessages[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition += new Vector2(0, rectTransform.rect.height + 10); // 메시지 간 간격 추가
        }

        // 새로운 메시지 표시
        StartCoroutine(DisplaySystemMessage(messageObject));
    }

    private IEnumerator DisplaySystemMessage(GameObject messageObject)
    {
        messageObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        messageObject.SetActive(false);
        activeMessages.Remove(messageObject);
        Destroy(messageObject);
    }
}
