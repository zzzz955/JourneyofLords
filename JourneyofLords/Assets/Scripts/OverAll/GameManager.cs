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
    public string heroDataFilePath = "Scripts/GameData/HeroData.xlsx"; // 상대 경로로 설정
    public string[] rateDataFilePaths = { "Scripts/GameData/HeroRecruit1.xlsx", "Scripts/GameData/HeroRecruit2.xlsx", "Scripts/GameData/HeroRecruit3.xlsx" }; // 상대 경로로 설정
    public string stageDataFilePath = "Scripts/GameData/StageData.xlsx";

    public HeroList HeroList { get; private set; }
    public List<List<HeroRate>> HeroRatesList { get; private set; }
    public List<StageData> StageDataList { get; private set; }
    public User CurrentUser { get; private set; }
    public MainUI MainUI { get; private set; }
    public HeroManager HeroManager { get; private set; }
    public Dictionary<int, Hero> SelectedHeroes { get; private set; } = new Dictionary<int, Hero>();
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

        Debug.Log("GameManager Awake 호출됨");

        if (Instance == this)
        {
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
        Debug.Log("OnHeroesLoaded called");

        HeroManager heroManager = FindObjectOfType<HeroManager>();
        if (heroManager != null)
        {
            if (CurrentUser != null)
            {
                Debug.Log("CurrentUser.maxHeroes: " + CurrentUser.maxHeroes);
                heroManager.MaxInitialize(CurrentUser.maxHeroes);
            }
            else
            {
                Debug.LogError("CurrentUser is null in OnHeroesLoaded");
            }
            heroManager.SetHeroList(HeroList);
            heroManager.SetHeroRatesList(HeroRatesList);
        }
        else
        {
            Debug.LogError("HeroManager is null in OnHeroesLoaded");
        }
    }

    private void LoadAllData()
    {
        HeroList = LoadHeroData(heroDataFilePath);
        HeroRatesList = LoadHeroRates(rateDataFilePaths);
        StageDataList = LoadStageData(stageDataFilePath);
        Debug.Log($"Loaded {StageDataList.Count} stages.");
        foreach (var stage in StageDataList)
        {
            Debug.Log($"Stage level: {stage.level}, Enemies count: {stage.enemies.Count}");
        }
    }

    public void SetUserData(User userData)
    {
        CurrentUser = userData;
    }

    private void SaveUserData()
    {
        firestoreManager.UpdateUserData(CurrentUser, 
            onSuccess: () => Debug.Log("User data updated successfully."),
            onFailure: (error) => Debug.LogError("Error updating user data: " + error));
    }

    private HeroList LoadHeroData(string filePath)
    {
        string fullPath = Path.Combine(Application.dataPath, filePath);
        ExcelDataLoader dataLoader = ScriptableObject.CreateInstance<ExcelDataLoader>();
        dataLoader.Initialize(heroDataFilePath, rateDataFilePaths);
        return dataLoader.LoadHeroData(fullPath);
    }

    private List<List<HeroRate>> LoadHeroRates(string[] filePaths)
    {
        List<List<HeroRate>> heroRatesList = new List<List<HeroRate>>();
        ExcelDataLoader dataLoader = ScriptableObject.CreateInstance<ExcelDataLoader>();
        dataLoader.Initialize(heroDataFilePath, filePaths);
        foreach (var filePath in filePaths)
        {
            string fullPath = Path.Combine(Application.dataPath, filePath);
            heroRatesList.Add(dataLoader.LoadRateData(fullPath));
        }
        return heroRatesList;
    }

    private List<StageData> LoadStageData(string filePath)
    {
        string fullPath = Path.Combine(Application.dataPath, filePath);
        ExcelDataLoader dataLoader = ScriptableObject.CreateInstance<ExcelDataLoader>();
        dataLoader.Initialize(heroDataFilePath, rateDataFilePaths);
        return dataLoader.LoadStageData(fullPath);
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
