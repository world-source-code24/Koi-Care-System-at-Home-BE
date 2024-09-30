using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class KoiRepository : GenericRepository<KoisTbl>, IKoiRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public KoiRepository(KoiCareSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<KoisTbl>> GetKoiByPondIdAsync(int pondId)
        {
            var ListOfKoi = await _context.KoisTbls.Where(p => p.PondId == pondId)
                .Select(p => new KoisTbl
                {
                    KoiId = p.KoiId,
                    Name = p.Name,
                    Image = p.Image,
                    Physique = p.Physique,
                    Age = p.Age,
                    Length = p.Length,
                    Weight = p.Weight,
                    Sex = p.Sex,
                    Breed = p.Breed,
                }).ToListAsync();
            return ListOfKoi;
        }

        public async Task<KoisTbl> GetKoiByKoiIdAsync(int koiId)
        {
            var koiDetails = await _context.KoisTbls
                .Where(p => p.KoiId == koiId)
                .Select(p => new KoisTbl
                {
                    Name = p.Name,
                    Image = p.Image,
                    Physique = p.Physique,
                    Age = p.Age,
                    Length = p.Length,
                    Weight = p.Weight,
                    Sex = p.Sex,
                    Breed = p.Breed,
                }).FirstOrDefaultAsync();
            if(koiDetails == null)
            {
                return null;
            }
            return koiDetails;
        }

        public async Task<List<KoisTbl>> GetKoiByUserIdAsync(int userId)
        {
            var ListOfKoi = await (from koi in _context.KoisTbls
                                   join pond in _context.PondsTbls on koi.PondId equals pond.PondId
                                   where pond.UserId == userId
                                   select new KoisTbl
                                   {
                                       KoiId = koi.KoiId,
                                       Name = koi.Name,
                                       Image = koi.Image,
                                       Physique = koi.Physique,
                                       Age = koi.Age,
                                       Length = koi.Length,
                                       Weight = koi.Weight,
                                       Sex = koi.Sex,
                                       Breed = koi.Breed,
                                   }).ToListAsync();
            return ListOfKoi;
        }

        public async Task SaveKoiToChartAsync(int koiId)
        {
        }
    }
}
