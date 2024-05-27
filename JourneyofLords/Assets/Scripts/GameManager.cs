using System;
using System.Collections;
using System.Collections.Generic; // Ensure this is included
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExcelDataReader;
using Firebase.Firestore;
using Firebase.Extensions;

public class GameManager : Singleton<GameManager>
{
    public string heroDataFilePath = "Scripts/GameData/HeroData.xlsx"; // 상대 경로로 설정
    public string[] rateDataFilePaths = { "Scripts/GameData/HeroRecruit1.xlsx", "Scripts/GameData/HeroRecruit2.xlsx", "Scripts/GameData/HeroRecruit3.xlsx" }; // 상대 경로로 설정

    public List<User> Users { get; private set; }
    public HeroList HeroList { get; private set; }
    public List<List<HeroRate>> HeroRatesList { get; private set; }
    public User CurrentUser { get; private set; }
    public MainUI MainUI { get; private set; }
    public HeroManager HeroManager { get; private set; }

    private FirestoreManager firestoreManager;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        firestoreManager = FindObjectOfType<FirestoreManager>();
        LoadAllData();
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

    public static HeroList LoadHeroData(string filePath)
    {
        string heroDataPath = Path.Combine(Application.dataPath, filePath);
        heroDataPath = heroDataPath.Replace("\\", "/");
        Debug.Log("Hero data path: " + heroDataPath);

        if (!File.Exists(heroDataPath))
        {
            Debug.LogError("Hero data file not found: " + heroDataPath);
            return null;
        }

        List<Hero> heroes = new List<Hero>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(heroDataPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[0];

                for (int i = 1; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];

                    Hero hero = new Hero
                    {
                        id = row[0].ToString(),
                        index = int.Parse(row[1].ToString()),
                        grade = int.Parse(row[2].ToString()),
                        rarity = int.Parse(row[3].ToString()),
                        att = row[4].ToString(),
                        name = row[5].ToString(),
                        sex = row[6].ToString(),
                        level = int.Parse(row[7].ToString()),
                        exp = int.Parse(row[8].ToString()),
                        rebirth = int.Parse(row[9].ToString()),
                        growth = int.Parse(row[10].ToString()),
                        atk = float.Parse(row[11].ToString()),
                        def = float.Parse(row[12].ToString()),
                        hp = float.Parse(row[13].ToString()),
                        lead = float.Parse(row[14].ToString()),
                        spriteName = row[15].ToString(),
                        equip = new List<string>(row[16].ToString().Split(';')),
                        description = row[17].ToString(),
                    };

                    heroes.Add(hero);
                }
            }
        }
        HeroList heroList = ScriptableObject.CreateInstance<HeroList>();
        heroList.heroes = heroes;
        return heroList;
    }

    public static List<List<HeroRate>> LoadHeroRates(string[] filePaths)
    {
        List<List<HeroRate>> heroRatesList = new List<List<HeroRate>>();

        foreach (var filePath in filePaths)
        {
            string rateDataPath = Path.Combine(Application.dataPath, filePath);
            rateDataPath = rateDataPath.Replace("\\", "/");
            Debug.Log("Rate data path: " + rateDataPath);

            if (!File.Exists(rateDataPath))
            {
                Debug.LogError("Rate data file not found: " + rateDataPath);
                continue;
            }

            List<HeroRate> heroRates = new List<HeroRate>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(rateDataPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];

                        HeroRate heroRate = new HeroRate
                        {
                            index = int.Parse(row[0].ToString()),
                            rate = float.Parse(row[1].ToString())
                        };

                        heroRates.Add(heroRate);
                    }
                }
            }

            heroRatesList.Add(heroRates);
        }

        return heroRatesList;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
