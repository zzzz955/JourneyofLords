using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
    public string filePath = "Assets/Resources/heroes.xlsx"; // 엑셀 파일 경로

    public List<Hero> ReadExcel()
    {
        List<Hero> heroes = new List<Hero>();

        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        DataTable table = result.Tables[0];
        for (int i = 1; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            Hero hero = new Hero();
            hero.ID = int.Parse(row[0].ToString());
            hero.Name = row[1].ToString();
            hero.Level = int.Parse(row[2].ToString());
            hero.ImagePath = row[3].ToString();
            heroes.Add(hero);
        }

        excelReader.Close();
        return heroes;
    }
}

public class Hero
{
    public int ID;
    public string Name;
    public int Level;
    public string ImagePath;
}
