using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using ExcelDataReader;

public class ExcelDataLoader : MonoBehaviour
{
    public string heroDataFilePath; // 영웅 정보 파일 경로
    public string[] rateDataFilePaths; // 확률 정보 파일 경로

    private List<User> users = new List<User>();
    private HeroList heroList;
    private List<List<HeroRate>> heroRatesList = new List<List<HeroRate>>();

    void Start()
    {
        string heroDataPath = Path.Combine(Application.dataPath, heroDataFilePath);
        heroList = LoadHeroData(heroDataPath);

        foreach (string rateDataFilePath in rateDataFilePaths)
        {
            string rateDataPath = Path.Combine(Application.dataPath, rateDataFilePath);
            List<HeroRate> heroRates = LoadRateData(rateDataPath);
            heroRatesList.Add(heroRates);
        }

        HeroManager heroManager = FindObjectOfType<HeroManager>();
        if (heroManager != null)
        {
            heroManager.SetHeroList(heroList); // 모든 영웅 데이터를 초기화
            heroManager.SetHeroRatesList(heroRatesList); // 각 파일의 확률 리스트를 설정
        }
    }

    public HeroList LoadHeroData(string excelFilePath)
    {
        List<Hero> heroes = new List<Hero>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
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

    public List<HeroRate> LoadRateData(string excelFilePath)
    {
        List<HeroRate> heroRates = new List<HeroRate>();
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
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
        return heroRates;
    }

    public List<StageData> LoadStageData(string excelFilePath)
    {
        List<StageData> stages = new List<StageData>();
        Dictionary<int, StageData> stageDictionary = new Dictionary<int, StageData>();

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[0];

                for (int i = 1; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];

                    try
                    {
                        int stageLevel = int.Parse(row[0].ToString());
                        int enemyIndex = int.Parse(row[1].ToString());
                        int enemyPos = int.Parse(row[2].ToString());
                        Vector2Int position = ConvertToGridPosition(enemyPos);
                        int heroIndex = int.Parse(row[3].ToString());
                        int enemyLevel = int.Parse(row[4].ToString());
                        float atk = float.Parse(row[5].ToString());
                        float def = float.Parse(row[6].ToString());
                        float hp = float.Parse(row[7].ToString());
                        float lead = float.Parse(row[8].ToString());

                        Debug.Log($"Stage: {stageLevel}, EnemyIndex: {enemyIndex}, Position: {position}, HeroIndex: {heroIndex}");

                        Hero baseHero = GameManager.Instance.HeroList.heroes.Find(hero => hero.index == heroIndex);
                        if (baseHero != null)
                        {
                            Hero enemyHero = new Hero
                            {
                                id = baseHero.id,
                                index = baseHero.index,
                                grade = baseHero.grade,
                                rarity = baseHero.rarity,
                                att = baseHero.att,
                                name = baseHero.name,
                                sex = baseHero.sex,
                                level = enemyLevel,
                                exp = baseHero.exp,
                                rebirth = baseHero.rebirth,
                                growth = baseHero.growth,
                                atk = atk,
                                def = def,
                                hp = hp,
                                lead = lead,
                                spriteName = baseHero.spriteName,
                                equip = new List<string>(baseHero.equip),
                                description = baseHero.description
                            };

                            if (!stageDictionary.ContainsKey(stageLevel))
                            {
                                stageDictionary[stageLevel] = new StageData { level = stageLevel, enemies = new List<Enemy>() };
                            }

                            stageDictionary[stageLevel].enemies.Add(new Enemy(position, enemyHero));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error processing row {i}: {ex.Message}");
                    }
                }
            }
        }

        foreach (var kvp in stageDictionary)
        {
            Debug.Log($"Adding stage level {kvp.Key} with {kvp.Value.enemies.Count} enemies.");
            stages.Add(kvp.Value);
        }

        return stages;
    }

    private Vector2Int ConvertToGridPosition(int position)
    {
        switch (position)
        {
            case 1: return new Vector2Int(0, 0);
            case 2: return new Vector2Int(1, 0);
            case 3: return new Vector2Int(2, 0);
            case 4: return new Vector2Int(0, 1);
            case 5: return new Vector2Int(1, 1);
            case 6: return new Vector2Int(2, 1);
            case 7: return new Vector2Int(0, 2);
            case 8: return new Vector2Int(1, 2);
            case 9: return new Vector2Int(2, 2);
            default: return new Vector2Int(-1, -1); // Invalid position
        }
    }
}

