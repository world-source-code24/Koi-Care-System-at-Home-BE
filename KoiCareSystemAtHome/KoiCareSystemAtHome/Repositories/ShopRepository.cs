using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class ShopRepository : GenericRepository<ShopsTbl>, IShopRepository
    {
        private readonly KoicareathomeContext _context;
        public ShopRepository (KoicareathomeContext context) : base(context)
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
                Email = s.Email,
                ShopCode = s.ShopCode,
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
                Email= s.Email,
                ShopCode = s.ShopCode,
            }).SingleOrDefaultAsync();
        }

        public async Task<ShopsTbl> GetShopByCode(string code)
        {
            var shop = await _context.ShopsTbls.FirstOrDefaultAsync(s => s.ShopCode.Equals(code));
            return shop;
        }

        public async Task<int> GetTotalShops()
        {
            return await _context.ShopsTbls.CountAsync();
        }
    }
}
