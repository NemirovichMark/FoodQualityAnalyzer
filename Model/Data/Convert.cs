using System.IO;
using System.Xml.Serialization;
using Model.Core;
using Newtonsoft.Json.Linq;
using static Model.Data.SerializeXML;

namespace Model.Data;

public class Convert
{
    public static void ConvertFromJsonToXML(string folderpath)
    {
        if (!Directory.Exists(folderpath)) return;
        var SerX = new SerializeXML();
        var files = Directory.GetFiles(folderpath);
        foreach (var file in files)
        {
            var newfilename = Path.GetFileNameWithoutExtension(file);
            var newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsXML",
                newfilename + ".xml");
            if (File.Exists(newpath)) continue;

            var text = File.ReadAllText(file);
            var obj = JObject.Parse(text);

            var prods = new List<FoodProduct>();

            foreach (var p in obj["Products"])
            {
                var prod = SerializeJson.Deser(p);
                prods.Add(prod);
            }

            SerX.GenerateReport(prods.ToArray(), newfilename);
        }
    }

    public static void ConvertFromXMLToJson(string folderpath)
    {
        if (!Directory.Exists(folderpath)) return;
        var SerJ = new SerializeJson();
        var files = Directory.GetFiles(folderpath);
        foreach (var file in files)
        {
            var newfilename = Path.GetFileNameWithoutExtension(file);
            var newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsJson",
                newfilename + ".json");
            if (File.Exists(newpath)) continue;

            using var reader = new StreamReader(file);
            var ser = new XmlSerializer(typeof(ReportDTO));
            var obj = (ReportDTO)ser.Deserialize(reader);

            var prods = new List<FoodProduct>();

            foreach (var p in obj.Products)
            {
                FoodProduct product = p switch
                {
                    VegetableDTO v => new Vegetable(v.Name, v.DaysUntilBad, v.MaxLifeDays, v.NeedToCook, v.IsHard),
                    FruitDTO f => new Fruit(f.Name, f.DaysUntilBad, f.MaxLifeDays, f.IsPopular, f.IsExhotic),
                    MeatDTO m => new Meat(m.Name, m.DaysUntilBad, m.MaxLifeDays, m.IsRed, m.WithBones),
                    BackeryDTO b => new Backery(b.Name, b.DaysUntilBad, b.MaxLifeDays, b.IsSweet, b.IsBread),
                    _ => null
                };

                prods.Add(product);
                SerJ.GenerateReport(prods.ToArray(), newfilename);
            }
        }
    }
}