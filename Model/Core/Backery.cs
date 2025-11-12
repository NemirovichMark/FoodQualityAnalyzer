namespace Model.Core;

public class Backery : FoodProduct
{
    public Backery(string name, int daysUntilBad, int maxLifeDays, bool isSweet, bool isBread)
        : base(name, daysUntilBad, maxLifeDays)
    {
        IsSweet = isSweet;
        IsBread = isBread;
        Type = nameof(Backery);
    }

    public bool IsSweet { get; private set; }
    public bool IsBread { get; private set; }
    public string Type { get; private set; }

    public override double GetQuality() //переопределение 1
    {
        var quality = DaysUntilBad / (double)MaxLifeDays * 100;
        return Math.Round(quality, 2);
    }
}