namespace YakShop.DTOs
{
    public class OrderDto
    {
        public string Customer { get; set; } = default!;
        public double? MilkOrdered { get; set; }
        public int? SkinsOrdered { get; set; }
    }
}
