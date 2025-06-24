namespace YakShop.Api.Entities;

public class Yak
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public double AgeInYears { get; set; }
    public string Sex { get; set; } = default!;

    public double AgeInDays => AgeInYears * 100;
    public double AgeLastShavedInDays { get; set; }

    public bool IsAlive => AgeInDays < 1000;
}