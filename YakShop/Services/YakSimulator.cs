using YakShop.Entities;
using YakShop.Models;

namespace YakShop.Services;

public class YakSimulator
{
    // Function for calculating the simulationr results of a given herd and day   
    public SimulationResult Simulate(List<Yak> yaks, int targetDay)
    {
        double milk = 0;
        int skins = 0;

        //Yaks are simulated as InternalYakState to track their state during the simulation.
        var internalHerd = yaks
            .Select(y => new InternalYakState
            {
                Name = y.Name,
                AgeInDays = y.AgeInDays,
                LastShavedSimDay = 0,
                LastShavedAgeInYears = y.AgeLastShavedInDays / 100
            }).ToList();

        for (int day = 0; day < targetDay; day++)
        {
            foreach (var yak in internalHerd)
            {
                if (yak.AgeInDays >= 1000) continue;

                //Milking
                double todaysMilk = 50 - yak.AgeInDays * 0.03;
                if (todaysMilk > 0)
                    milk += todaysMilk;

                //Shaving
                double minDaysBetweenShaves = 8 + yak.AgeInDays * 0.01;
                if (yak.AgeInDays >= 100 && (day - yak.LastShavedSimDay) >= minDaysBetweenShaves)
                {
                    skins++;
                    yak.LastShavedSimDay = day;
                    yak.LastShavedAgeInYears = yak.AgeInDays / 100;
                }

                yak.AgeInDays++;
            }
        }

        // Prepare the result with total milk, total skins, and the state of the herd.
        var result = new SimulationResult
        {
            TotalMilk = Math.Round(milk, 2),
            TotalSkins = skins,
            Herd = internalHerd.Select(y => new SimulatedYak
            {
                Name = y.Name,
                AgeInYears = Math.Round(y.AgeInDays / 100, 2),
                AgeLastShaved = Math.Round(y.LastShavedAgeInYears, 2)
            }).ToList()
        };

        return result;
    }
}
