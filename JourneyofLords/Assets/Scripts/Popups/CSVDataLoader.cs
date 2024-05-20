using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVDataLoader : MonoBehaviour
{
    public string csvFilePath = "Assets/Scripts/GameData/HeroData.csv";

    void Start()
    {
        HeroList heroList = LoadCSVData(csvFilePath);
        HeroManager heroManager = FindObjectOfType<HeroManager>();
        if (heroManager != null)
        {
            heroManager.Initialize(heroList);
        }
    }

    public static HeroList LoadCSVData(string csvFilePath)
    {
        List<Hero> heroes = new List<Hero>();
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            bool isFirstLine = true;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                string[] values = line.Split(',');

                Hero hero = new Hero
                {
                    id = values[0],
                    grade = int.Parse(values[1]),
                    att = values[2],
                    name = values[3],
                    level = int.Parse(values[4]),
                    exp = int.Parse(values[5]),
                    rebirth = int.Parse(values[6]),
                    growth = int.Parse(values[7]),
                    atk = float.Parse(values[8]),
                    def = float.Parse(values[9]),
                    hp = float.Parse(values[10]),
                    lead = float.Parse(values[11]),
                    spriteName = values[12],
                    equip = new List<string>(values[13].Split(';')) // CSV에서 아이템 목록을 ';'로 구분한 경우
                };

                heroes.Add(hero);
            }
        }

        HeroList heroList = ScriptableObject.CreateInstance<HeroList>();
        heroList.heroes = heroes;

        return heroList;
    }
}
