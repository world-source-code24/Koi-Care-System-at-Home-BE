using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class KoiChartRepository : GenericRepository<KoiGrowthChartsTbl>, IKoiChartRepository
    {
        private readonly KoicareathomeContext _context;
        public KoiChartRepository(KoicareathomeContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<KoiGrowthChartsTbl>> GetKoiGrowthCharts(int koiId)
        {
            var koiChart = await (from chart in _context.KoiGrowthChartsTbls
                                  join koi in _context.KoisTbls on chart.KoiId equals koi.KoiId
                                  where koi.KoiId == koiId
                                  select new KoiGrowthChartsTbl
                                  {
                                      Date = chart.Date,
                                      Length = chart.Length,
                                      Weight = chart.Weight,
                                      HealthStatus = chart.HealthStatus,
                                      ChartId = chart.ChartId,
                                      KoiId = koiId
                                  }).ToListAsync();
            return koiChart;
        }
    }
}
