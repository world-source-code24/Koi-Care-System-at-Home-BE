using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<AccountTbl>
    {
        //Get Profile Account by id
        Task<AccountDTO> GetAccountProfile(int id);

        //Update Profile
        Task<bool> UpdateProfile(int id, AccountDTO updateInformation);
        Task<List<AccountTbl>> GetAllAccounts();
        Task<List<AccountTbl>> GetAllAccountsByRole(string role);

    }
}
