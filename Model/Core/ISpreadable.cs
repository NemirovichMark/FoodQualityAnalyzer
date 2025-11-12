namespace Model.Core;

public interface ISpreadable //1 интерфейс
{
    public static FoodProduct[] Products { get; }

    void Add(FoodProduct product)
    {
    }

    void Add(FoodProduct[] products)
    {
    } //перегрузка 1
}