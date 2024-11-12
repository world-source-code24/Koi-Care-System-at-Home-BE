using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorRepository _calculatorRepository;
        private readonly KoicareathomeContext _koicareathomeContext;
        public CalculatorController(ICalculatorRepository calculatorRepository, KoicareathomeContext koicareathomeContext)
        {
            _calculatorRepository = calculatorRepository;
            _koicareathomeContext = koicareathomeContext;
        }
        [HttpGet("food-calculator")]
        public async Task<IActionResult> FoodCalculator(int pondId, string growthLevel, decimal minTemp, decimal maxTemp)
        {
            var result = await _calculatorRepository.foodCalculator(pondId, growthLevel, minTemp, maxTemp);
            string advie = await _calculatorRepository.adviceFood(growthLevel);
            return Ok(new {recommendFood = result, adive = advie});
        }

        [HttpGet("parameter")]
        public async Task<IActionResult> GetFoodParameter()
        {
            var parameterList = _koicareathomeContext.FoodCalculateParameters;
            return Ok(parameterList);
        }

        [HttpGet("rangeTemp")]
        public async Task<IActionResult> GetRangeTemp()
        {
            var temperatureRanges = _koicareathomeContext.TemperatureRanges;
            return Ok(temperatureRanges);
        }

        [HttpPut("parameter")]
        public async Task<IActionResult> FoodParameter(int id, float multiplierLower, float multiplierBetween, float multiplierUpper, string advice)
        {
            var newUpdate = _koicareathomeContext.FoodCalculateParameters.FirstOrDefault(f => f.ParameterId.Equals(id));
            if (newUpdate != null)
            {
                newUpdate.MultiplierUpper = multiplierUpper;
                newUpdate.MultiplierLower = multiplierLower;
                newUpdate.MultiplierBetween = multiplierBetween;
                newUpdate.Advice = advice;
                _koicareathomeContext.SaveChanges();
                return Ok("Successfull!");
            }
            else
            {
                return BadRequest("Update fail!");
            }
        }

        [HttpPut("rangeTemp")]
        public async Task<IActionResult> TemperatureRange(int id, decimal minTemp, decimal maxTemp)
        {
            var newUpdate = _koicareathomeContext.TemperatureRanges.FirstOrDefault(t => t.RangeId == id);
            if (newUpdate != null)
            {
                newUpdate.MinTemp = minTemp;
                newUpdate.MaxTemp = maxTemp;
                _koicareathomeContext.SaveChanges();
                return Ok("Successfull!");
            }
            else
            {
                return BadRequest("Update fail!");
            }
        }
    }
}
