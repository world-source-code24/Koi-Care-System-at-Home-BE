using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface ICalculatorRepository
    {
        Task<decimal> foodCalculator(int pondId, string growthLevel, decimal downTemp, decimal upTemp);
        Task<string> adviceFood(string growthLevel);
    }
}
