namespace YakShop.Models
{
    public class SimulationResult
    {
        public double TotalMilk { get; set; }
        public int TotalSkins { get; set; }
        public List<SimulatedYak> Herd { get; set; } = new();
    }
}
