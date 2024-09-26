using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class WaterParameterRepository : GenericRepository<WaterParametersTbl>, IWaterParameterRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public WaterParameterRepository(KoiCareSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<WaterParametersTbl>> GetAllByPondIdAsync(int pondId)
        {

            var results = await _context.WaterParametersTbls.Where(p => p.PondId == pondId)
                .Select(p => new WaterParametersTbl
                {
             
                    ParameterId = p.ParameterId,

                    Temperature = p.Temperature,

                    Salt = p.Salt,

                    PhLevel = p.PhLevel,

                    O2Level = p.O2Level,

                    No2Level = p.No2Level,

                    No3Level = p.No3Level,

                    Po4Level = p.Po4Level,

                    TotalChlorines = p.TotalChlorines,

                    Date = DateTime.Now,

                    Note = p.Note,

                    PondId = p.PondId,
                }).ToListAsync();
            return results;
        }



        public async Task<WaterParametersTbl> GetByPondIdAsync(int id)
        {
            var result = await _context.WaterParametersTbls
                .Where(p => p.PondId == id)
                .OrderByDescending(p => p.Date)
                .Select(p => new WaterParametersTbl
                {
                    ParameterId = p.ParameterId,

                    Temperature = p.Temperature,

                    Salt = p.Salt,

                    PhLevel = p.PhLevel,

                    O2Level = p.O2Level,

                    No2Level = p.No2Level,

                    No3Level = p.No3Level,

                    Po4Level = p.Po4Level,

                    TotalChlorines = p.TotalChlorines,

                    Date = DateTime.Now,

                    Note = p.Note,

                    PondId = p.PondId,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
