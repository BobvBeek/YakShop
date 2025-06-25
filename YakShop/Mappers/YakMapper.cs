using YakShop.DTOs;
using YakShop.Entities;

namespace YakShop.Mappers
{
    public static class YakMapper
    {
        public static YakDto ToDto(Yak yak) => new YakDto
        {
            Name = yak.Name,
            Age = yak.AgeInYears,
            Sex = yak.Sex
        };

        public static Yak FromDto(YakDto dto) => new Yak
        {
            Name = dto.Name,
            AgeInYears = dto.Age,
            Sex = dto.Sex
        };
    }
}
