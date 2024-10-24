using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IMembershipRepository
    {
        Task<List<MembershipRevenue>> GetRenevueMembership();
    }
}
