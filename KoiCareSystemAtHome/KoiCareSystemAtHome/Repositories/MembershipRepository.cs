using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly KoicareathomeContext _context;

        public MembershipRepository(KoicareathomeContext context)
        {
            _context = context;
        }

        public async Task<List<MembershipRevenue>> GetRenevueMembership()
        {
            var memberRevenue = await _context.MembershipDashboards.ToListAsync();
            var monthlyRevenue = memberRevenue
                .GroupBy(m => new { Year = m.StartDate.Year, Month = m.StartDate.Month })
                .Select(group => new MembershipRevenue
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    TotalRevenue = group.Sum(m => m.Money)
                })
                .OrderBy(result => result.Year)
                .ThenBy(result => result.Month)
                .ToList();

            return monthlyRevenue;
        }

        public async Task<decimal> GetTotalRenevueMembership()
        {
            decimal totalRenevueMembership = await _context.MembershipDashboards.SumAsync(m => m.Money);
            return totalRenevueMembership;
        }
    }
}
