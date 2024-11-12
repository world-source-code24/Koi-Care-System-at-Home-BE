using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorRepository _calculatorRepository;
        public CalculatorController(ICalculatorRepository calculatorRepository)
        {
            _calculatorRepository = calculatorRepository;
        }
        [HttpGet("food-calculator")]
        public async Task<IActionResult> FoodCalculator(int pondId, string growthLevel, decimal minTemp, decimal maxTemp)
        {
            var result = await _calculatorRepository.foodCalculator(pondId, growthLevel, minTemp, maxTemp);
            string advie = await _calculatorRepository.adviceFood(growthLevel);
            return Ok(new {recommendFood = result, adive = advie});
        }
    }
}
