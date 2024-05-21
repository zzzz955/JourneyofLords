using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;

public class ExcelDataLoader : MonoBehaviour
{
    public string heroDataFilePath; // 영웅 정보 파일 경로
    public string[] rateDataFilePaths; // 확률 정보 파일 경로

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
            heroManager.Initialize(heroList); // 모든 영웅 데이터를 초기화
            heroManager.SetHeroRatesList(heroRatesList); // 각 파일의 확률 리스트를 설정
        }
    }

    public HeroList LoadHeroData(string excelFilePath)
    {
        List<Hero> heroes = new List<Hero>();

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
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
                        equip = new List<string>(row[16].ToString().Split(';')), // Excel에서 아이템 목록을 ';'로 구분한 경우
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
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
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
}

public class HeroRate
{
    public int index;
    public float rate;
}
