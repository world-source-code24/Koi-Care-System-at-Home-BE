using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using KoiCareSystemAtHome.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KoiCareSystemAtHome.Repositories
{
    public class KoiRepository : GenericRepository<KoisTbl>, IKoiRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public KoiRepository(KoiCareSystemDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<List<KoisTbl>> GetKoiByPondId(int pondId)
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

        public async Task<KoisTbl> GetKoiByKoiId(int koiId)
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
            return koiDetails;
        }

        public async Task<List<KoisTbl>> GetKoiByUserIdAndPondId(int userId, int pondId)
        {
            var ListOfPondIds = await _context.PondsTbls
                .Where(p => p.UserId == userId)
                .Select(p => p.PondId)
                .ToListAsync();
            
                var ListOfKoi = await _context.KoisTbls
                .Where(k => ListOfPondIds.Contains(pondId))
                .Select(k => new KoisTbl
                {
                    KoiId = k.KoiId,
                    Name = k.Name,
                    Image = k.Image,
                    Physique = k.Physique,
                    Age = k.Age,
                    Length = k.Length,
                    Weight = k.Weight,
                    Sex = k.Sex,
                    Breed = k.Breed,
                }).ToListAsync();
                return ListOfKoi;
        }

        public void SaveKoiToChart(int koiId)
        {
        }
    }
}
