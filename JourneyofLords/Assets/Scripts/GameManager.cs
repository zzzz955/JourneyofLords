using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExcelDataReader;

public class GameManager : Singleton<GameManager>
{
    public string userDataFilePath = "Scripts/GameData/UserData.xlsx"; // 상대 경로로 설정
    public string heroDataFilePath = "Scripts/GameData/HeroData.xlsx"; // 상대 경로로 설정
    public string[] rateDataFilePaths = { "Scripts/GameData/HeroRecruit1.xlsx", "Scripts/GameData/HeroRecruit2.xlsx", "Scripts/GameData/HeroRecruit3.xlsx" }; // 상대 경로로 설정

    public List<User> Users { get; private set; }
    public HeroList HeroList { get; private set; }
    public List<List<HeroRate>> HeroRatesList { get; private set; }
    public User CurrentUser { get; private set; }
    public MainUI MainUI { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

    private void LoadAllData()
    {
        Users = LoadUserData(userDataFilePath);
        HeroList = LoadHeroData(heroDataFilePath);
        HeroRatesList = LoadHeroRates(rateDataFilePaths);

        // HeroManager에 데이터 설정 (필요한 경우)
        HeroManager heroManager = FindObjectOfType<HeroManager>();
        if (heroManager != null)
        {
            heroManager.SetHeroList(HeroList);
            heroManager.SetHeroRatesList(HeroRatesList);
        }
    }

    public void SetUserData(User userData)
    {
        CurrentUser = userData;
    }

    public static List<User> LoadUserData(string filePath)
    {
        string userDataPath = Path.Combine(Application.dataPath, filePath);
        userDataPath = userDataPath.Replace("\\", "/");
        Debug.Log("User data path: " + userDataPath);

        if (!File.Exists(userDataPath))
        {
            Debug.LogError("User data file not found: " + userDataPath);
            return null;
        }

        List<User> users = new List<User>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using (var stream = File.Open(userDataPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var dataTable = result.Tables[0];

                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];
                    User user = new User
                    {
                        email = row[0].ToString(),
                        IGN = row[1].ToString(),
                        gold = int.Parse(row[2].ToString()),
                        userLV = int.Parse(row[3].ToString()),
                        userEXP = int.Parse(row[4].ToString()),
                        wood = int.Parse(row[5].ToString()),
                        stone = int.Parse(row[6].ToString()),
                        iron = int.Parse(row[7].ToString()),
                        food = int.Parse(row[8].ToString()),
                        max_Stage = int.Parse(row[9].ToString()),
                        max_Rewards = int.Parse(row[10].ToString()),
                        maxHeroes = int.Parse(row[11].ToString()),
                        money = int.Parse(row[12].ToString()),
                        troops = int.Parse(row[13].ToString())
                    };
                    users.Add(user);
                }
            }
        }

        return users;
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
