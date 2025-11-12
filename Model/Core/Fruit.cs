namespace Model.Core;

public class Fruit : FoodProduct
{
    public Fruit(string name, int daysUntilBad, int maxLifeDays, bool isPopular, bool isExhotic)
        : base(name, daysUntilBad, maxLifeDays)
    {
        IsPopular = isPopular;
        IsExhotic = isExhotic;
        Type = nameof(Fruit);
    }

    public bool IsPopular { get; private set; }
    public bool IsExhotic { get; private set; }
    public string Type { get; private set; }

    public override double GetQuality() //переопределение 2
    {
        var quality = DaysUntilBad / (double)MaxLifeDays * 100;
        return Math.Round(quality, 2);
    }
}