using Model.Core;

namespace Model;

public interface IStatistic //интерфейс 3
{
    double MaxQuality(FoodProduct[] products)
    {
        return 0;
    }

    double MinQuality(FoodProduct[] products)
    {
        return 0;
    } //перегрузка 3

    double AverageQuality(FoodProduct[] products)
    {
        return 0;
    }

    double MedianQuality(FoodProduct[] products)
    {
        return 0;
    } //перегрузка 4
}