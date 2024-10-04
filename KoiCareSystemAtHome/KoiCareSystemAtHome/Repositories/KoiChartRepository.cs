using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class KoiChartRepository : GenericRepository<KoiGrowthChartsTbl>, IKoiChartRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public KoiChartRepository(KoiCareSystemDbContext context) : base(context)
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

        //public async Task UpdateKoiGrowthCharts(int koiId, KoiDTO koiDTO)
        //{
        //    DateOnly nowDay = DateOnly.FromDateTime(DateTime.Now);
        //    var koiChart = await _context.KoiGrowthChartsTbls
        //        .Where(p => p.KoiId == koiId && p.Date == nowDay)
        //        .Select(p => p.Date).SingleOrDefaultAsync();
        //    if(koiChart != nowDay)
        //    {
                
        //    }
        //    else
        //    {

        //    }
        //}
    }
}
