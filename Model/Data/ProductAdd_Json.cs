using System.IO;
using Model.Core;
using Newtonsoft.Json.Linq;

namespace Model.Data;

public class ProductAdd_Json
{
    public static string FolderPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProductsJson");

    public static void LoadProducts()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
            var products = CreateProducts();
            SaveProducts(products);
        }

        var files = Directory.GetFiles(FolderPath, "*.json");

        foreach (var file in files)
        {
            var text = File.ReadAllText(file);
            var json = JToken.Parse(text);
            var NewProduct = SerializeJson.Deser(json);
            var analyzer = new Core.FoodQualityAnalyzer();
            analyzer.Add(NewProduct);
        }
    }

    // Сохранение каждого продукта в отдельный файл
    public static void SaveProducts(FoodProduct[] products)
    {
        foreach (var product in products) SaveProduct(product);
    }

    public static void SaveProduct(FoodProduct product)
    {
        var SerJ = new SerializeJson();
        SerJ.Ser(product);

        var SerX = new SerializeXML();
        SerX.Ser(product);
    }


    // Начальные продукты
    public static FoodProduct[] CreateProducts()
    {
        return new FoodProduct[]
        {
            new Vegetable("Морковь", 40, 90, false, true),
            new Vegetable("Огурец", 25, 30, false, false),
            new Vegetable("Картофель", 297, 300, true, true),
            new Vegetable("Лук", 35, 100, true, true),
            new Vegetable("Томат", 10, 20, false, false),
            new Vegetable("Тыква", 68, 90, true, true),
            new Vegetable("Кабачок", 12, 35, true, true),
            new Vegetable("Батат", 40, 200, true, true),
            new Vegetable("Сладкий перец", 18, 20, false, false),

            new Fruit("Яблоко", 40, 50, true, false),
            new Fruit("Груша", 4, 30, true, false),
            new Fruit("Клубника", 9, 10, true, false),
            new Fruit("Банан", 3, 10, true, false),
            new Fruit("Драконий фрукт", 7, 10, false, true),
            new Fruit("Манго", 1, 10, true, true),
            new Fruit("Ананас", 15, 15, false, true),
            new Fruit("Виноград", 3, 10, true, false),
            new Fruit("Арбуз", 13, 15, false, false),
            new Fruit("Персик", 4, 10, true, false),
            new Fruit("Джекфрут", 3, 5, false, true),

            new Meat("Курица", 7, 15, false, false),
            new Meat("Говядина", 2, 15, true, true),
            new Meat("Свинина", 3, 15, true, true),
            new Meat("Конина", 4, 15, true, true),
            new Meat("Оленина", 7, 15, true, true),
            new Meat("Крольчатина", 11, 15, false, true),
            new Meat("Медвежатина", 15, 15, true, false),
            new Meat("Индейка", 14, 15, false, false),
            new Meat("Баранина", 8, 15, true, true),

            new Backery("Черный хлеб", 7, 7, false, true),
            new Backery("Белый хлеб", 6, 7, false, true),
            new Backery("Батон", 5, 7, false, true),
            new Backery("Багет", 3, 7, false, true),
            new Backery("Кекс", 13, 30, true, false),
            new Backery("Пончик", 7, 7, true, false),
            new Backery("Печенье", 28, 30, true, false),
            new Backery("Синабонн", 1, 2, true, false),
            new Backery("Рулет с клубникой", 2, 7, true, false)
        };
    }
}