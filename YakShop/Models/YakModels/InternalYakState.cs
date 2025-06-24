namespace YakShop.Models.YakModels
{
    public class InternalYakState
    {
        public string Name { get; set; } = default!;
        public double AgeInDays { get; set; }
        public double LastShavedAgeInYears { get; set; }
        public int LastShavedSimDay { get; set; } = 0;
    }
}
