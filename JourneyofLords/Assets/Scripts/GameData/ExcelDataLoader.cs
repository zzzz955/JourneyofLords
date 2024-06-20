using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using ExcelDataReader;

public class ExcelDataLoader : ScriptableObject
{
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
                        spriteName = row[14].ToString(),
                        equip = new List<string>(row[15].ToString().Split(';')),
                        description = row[16].ToString()
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
                        int heroIndex = int.Parse(row[2].ToString());
                        int enemyLevel = int.Parse(row[3].ToString());
                        float atk = float.Parse(row[4].ToString());
                        float def = float.Parse(row[5].ToString());
                        float hp = float.Parse(row[6].ToString());

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
                                spriteName = baseHero.spriteName,
                                equip = new List<string>(baseHero.equip),
                                description = baseHero.description
                            };

                            Enemy enemy = new Enemy(enemyIndex, enemyHero);

                            if (!stageDictionary.ContainsKey(stageLevel))
                            {
                                stageDictionary[stageLevel] = new StageData(stageLevel, new List<Enemy>());
                            }

                            stageDictionary[stageLevel].enemies.Add(enemy);
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
            stages.Add(kvp.Value);
        }

        return stages;
    }

    public List<LevelData> LoadLevelData(string excelFilePath)
    {
        List<LevelData> levels = new List<LevelData>();
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
                        int level = int.Parse(row[0].ToString());
                        int needEXP = int.Parse(row[1].ToString());
                        LevelData leveldata = new LevelData(level, needEXP);
                        levels.Add(leveldata);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error processing row {i}: {ex.Message}");
                    }
                }
            }
        }
        return levels;
    }

    public List<StageEXP> LoadStageEXP(string excelFilePath)
    {
        List<StageEXP> stageEXPs = new List<StageEXP>();
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
                        int getEXP = int.Parse(row[1].ToString());
                        StageEXP stageEXP = new StageEXP(stageLevel, getEXP);
                        stageEXPs.Add(stageEXP);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error processing row {i}: {ex.Message}");
                    }
                }
            }
        }
        return stageEXPs;
    }

    public List<Equip> LoadEquipData(string excelFilePath)
    {
        List<Equip> equips = new List<Equip>();
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
                        Equip equip = new Equip
                        {
                            id = row[0].ToString(),
                            index = int.Parse(row[1].ToString()),
                            type = row[2].ToString(),
                            rarity = int.Parse(row[3].ToString()),
                            name = row[4].ToString(),
                            level = int.Parse(row[5].ToString()),
                            atk = float.Parse(row[6].ToString()),
                            def = float.Parse(row[7].ToString()),
                            hp = float.Parse(row[8].ToString()),
                            spriteName = row[9].ToString(),
                            description = row[10].ToString()
                        };
                        equips.Add(equip);
                }
            }
        }
        return equips;
    }
}
