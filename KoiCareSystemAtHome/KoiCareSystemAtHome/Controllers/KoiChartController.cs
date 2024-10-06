using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoiChartController : ControllerBase
    {
        private readonly IKoiChartRepository _koiChartRepository;
        private readonly KoiCareSystemDbContext _context;
        public KoiChartController(IKoiChartRepository koiChartRepository, KoiCareSystemDbContext context)
        {
            _koiChartRepository = koiChartRepository;
            _context = context;
        }

        [HttpGet("{koiId}")]
        public async Task<IActionResult> GetChartAsync(int koiId)
        {
            var chart = await _koiChartRepository.GetKoiGrowthCharts(koiId);
            if (chart == null)
            {
                return NotFound(new {message = "Don't have any Koi paramester"});
            }
            return Ok(chart);
        }
    }
}
