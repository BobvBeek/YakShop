using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YakShop.Api.Entities;
using YakShop.Api.Services;

namespace YakShop.Tests
{
    public class YakSimulatorTests
    {
        // Test for the YakSimulator class, which simulates yak herding and milk production.
        [Fact]
        public void Simulate_WithSingleYak_ReturnsCorrectMilkAndSkins()
        {      
            var yak = new LabYak { Name = "Yak1", AgeInYears = 4, Sex = "FEMALE" };
            var herd = new List<LabYak> { yak };
            var simulator = new YakSimulator();

            var result = simulator.Simulate(herd, 13); // simulate for 13 days

            // Tests for: Not null, milk range, skins range, and herd size
            Assert.NotNull(result);
            Assert.InRange(result.TotalMilk, 0.0, 1000.0); // Milk should be positive and realistic
            Assert.InRange(result.TotalSkins, 0, 2);       // At most 1 shave in 13 days
            Assert.Single(result.Herd);
            Assert.Equal("Yak1", result.Herd[0].Name);
        }

        // Test for no milk production when yak is too old
        [Fact]
        public void Simulate_DeadYak_ProducesNoMilkOrSkins()
        {
            var yak = new LabYak { Name = "DeadYak", AgeInYears = 10.0, Sex = "FEMALE" };
            var simulator = new YakSimulator();
            var herd = new List<LabYak> { yak };

            var result = simulator.Simulate(herd, 10);

            // Tests for: No milk or skins produced
            Assert.Equal(0, result.TotalMilk);
            Assert.Equal(0, result.TotalSkins);
        }

        // Test whether the simulator correctly ages yaks over multiple days
        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(100)]
        public void Simulate_WithVariousDays_ProducesConsistentAging(int days)
        {
            var yak = new LabYak { Name = "AgingYak", AgeInYears = 5, Sex = "FEMALE" };
            var simulator = new YakSimulator();
            var herd = new List<LabYak> { yak };

            var result = simulator.Simulate(herd, days);

            // Test for: Age in range
            var expectedAge = yak.AgeInYears + (days / 100.0);
            Assert.InRange(result.Herd[0].AgeInYears, expectedAge - 0.01, expectedAge + 0.01);
        }
    }
}
