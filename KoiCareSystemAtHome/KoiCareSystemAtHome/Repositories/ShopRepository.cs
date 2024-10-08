using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class ShopRepository : GenericRepository<ShopsTbl>, IShopRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public ShopRepository (KoiCareSystemDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ShopsTbl>> GetAllShopsAsync()
        {
            return await _context.ShopsTbls.Select(s => new ShopsTbl
            {
                ShopId = s.ShopId,
                Address = s.Address,
                Name = s.Name,
                Phone = s.Phone,
            }).ToListAsync();
        }

        public async Task<ShopsTbl> GetByShopIdAsync(int shopId)
        {
            return await _context.ShopsTbls.Where(s => shopId.Equals(s.ShopId)).Select(s => new ShopsTbl
            {
                ShopId = s.ShopId,
                Address = s.Address,
                Name = s.Name,
                Phone = s.Phone,
            }).SingleOrDefaultAsync();
        }
    }
}
