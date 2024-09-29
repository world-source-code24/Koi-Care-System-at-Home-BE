using KoiCareSystemAtHome.Entities;
using System.Security.Claims;

namespace KoiCareSystemAtHome.Repositories.IRepositories

{
    public interface IUserRepository
    {
        Task<UserTbl> GetUserByEmailAsync(string email);
        string CreateRandomPassword();

        Task<(UserTbl, bool)> RegisterUserByEmailAsync(string email);
        Task SaveUserAsync(UserTbl user);
        Task<bool> CheckPhoneNumber(string phoneNumber);
        int GetCurrentUserId(ClaimsPrincipal user);
    }
}
