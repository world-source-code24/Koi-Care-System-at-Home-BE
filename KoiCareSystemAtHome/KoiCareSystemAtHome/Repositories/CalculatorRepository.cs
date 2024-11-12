using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class CalculatorRepository : ICalculatorRepository
    {
        private readonly KoicareathomeContext _context;
        public CalculatorRepository( KoicareathomeContext context)
        {
            _context = context;
        }

        public async Task<string> adviceFood(string growthLevel)
        {
            return await _context.FoodCalculateParameters.Where(f => f.Level.Equals(growthLevel)).Select(f => f.Advice).FirstOrDefaultAsync();
        }

        public async Task<decimal> foodCalculator(int pondId, string growthLevel, decimal downTemp, decimal upTemp)
        {
            var temperatureRanges = _context.TemperatureRanges.ToList();
            var levelCalculator = _context.FoodCalculateParameters.FirstOrDefault(f => f.Level.Equals(growthLevel));

            decimal weight = await _context.KoisTbls.Where(k => k.PondId == pondId).SumAsync(k => k.Weight);
            decimal avgTemp = (downTemp + upTemp) / 2;
            decimal recommendedAmount = 0;

            if (avgTemp < temperatureRanges.Min(t => t.MinTemp))
            {
                recommendedAmount = weight * (decimal)levelCalculator.MultiplierLower;
            }
            else if (temperatureRanges.Any(t => avgTemp >= t.MinTemp && avgTemp <= t.MaxTemp))
            {
                recommendedAmount = weight * (decimal)levelCalculator.MultiplierBetween;
            }
            else if (avgTemp > temperatureRanges.Max(t => t.MaxTemp))
            {
                recommendedAmount = weight * (decimal)levelCalculator.MultiplierUpper;
            }

            return recommendedAmount;
        }
    }
}
