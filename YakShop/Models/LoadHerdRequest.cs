namespace YakShop.Api.Models;

public class LoadHerdRequest
{
    public List<LabYakDto> Herd { get; set; } = new List<LabYakDto>();
}

public class LabYakDto
{
    public string Name { get; set; } = default!;
    public double Age { get; set; } 
    public string Sex { get; set; } = default!;
}
