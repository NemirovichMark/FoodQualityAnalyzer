using System.IO;
using Model.Core;
using Newtonsoft.Json.Linq;

namespace Model.Data;

public class SerializeJson : Serialize
{
    private static int _lastReportNumber;

    public override string Extension => "json"; //переопределение 5

    public override void Ser(FoodProduct product) //переопределение 6
    {
        SelectFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProductsJson"));
        SelectFile(product.Name);

        var json = JObject.FromObject(product);
        File.WriteAllText(FilePath, json.ToString());
    }

    public override void GenerateReport(FoodProduct[] products) //переопределение 7
    {
        SelectFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsJson"));

        _lastReportNumber = GetLastReportNumber();
        _lastReportNumber++;

        var time = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var FileName = $"Отчет_№{_lastReportNumber}_от_{time}";

        GenerateReport(products, FileName);
    }

    public override void GenerateReport(FoodProduct[] products, string filename) //переопределение 8
    {
        SelectFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReportsJson"));

        SelectFile(filename);

        // Сохраняем данные в JSON
        var data = new
        {
            Products = products
        };
        var json = JObject.FromObject(data);
        var analyzer = new Core.FoodQualityAnalyzer();
        json.Add("MaxQuality", analyzer.MaxQuality(products));
        json.Add("MinQuality", analyzer.MinQuality(products));
        json.Add("AverageQuality", analyzer.AverageQuality(products));
        json.Add("MedianQuality", analyzer.MedianQuality(products));
        File.WriteAllText(FilePath, json.ToString());
    }

    public static FoodProduct Deser(JToken json)
    {
        FoodProduct NewProduct = null;

        switch (json["Type"].ToString())
        {
            case nameof(Vegetable):
                NewProduct = new Vegetable(json["Name"].ToString(), // Приведение к FoodProduct 2
                    int.Parse(json["DaysUntilBad"].ToString()),
                    int.Parse(json["MaxLifeDays"].ToString()),
                    bool.Parse(json["NeedToCook"].ToString()),
                    bool.Parse(json["IsHard"].ToString()));
                break;
            case nameof(Fruit):
                NewProduct = new Fruit(json["Name"].ToString(), // Приведение к FoodProduct 3
                    int.Parse(json["DaysUntilBad"].ToString()),
                    int.Parse(json["MaxLifeDays"].ToString()),
                    bool.Parse(json["IsPopular"].ToString()),
                    bool.Parse(json["IsExhotic"].ToString()));
                break;
            case nameof(Meat):
                NewProduct = new Meat(json["Name"].ToString(), // Приведение к FoodProduct 4
                    int.Parse(json["DaysUntilBad"].ToString()),
                    int.Parse(json["MaxLifeDays"].ToString()),
                    bool.Parse(json["IsRed"].ToString()),
                    bool.Parse(json["WithBones"].ToString()));
                break;
            case nameof(Backery):
                NewProduct = new Backery(json["Name"].ToString(), // Приведение к FoodProduct 5
                    int.Parse(json["DaysUntilBad"].ToString()),
                    int.Parse(json["MaxLifeDays"].ToString()),
                    bool.Parse(json["IsSweet"].ToString()),
                    bool.Parse(json["IsBread"].ToString()));
                break;
        }

        return NewProduct;
    }
}