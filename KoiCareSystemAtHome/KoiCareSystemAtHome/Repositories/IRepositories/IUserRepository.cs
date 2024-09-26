using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<UserTbl> GetUserByEmailAsync(string email);
        string CreateRandomPassword();

        Task<(UserTbl, bool)> RegisterUserByEmailAsync(string email);
        Task SaveUserAsync(UserTbl user);
    }
}
