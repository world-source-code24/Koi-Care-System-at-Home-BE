using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<AccountTbl>
    {
        //Tao tai khoan moi
        Task<bool> RegisterAccount(AccountDTO account);

        //Get Profile Account by id
        Task<AccountDTO> GetAccountProfile(int id);
        //Update Profile
        Task<bool> UpdateProfile(int id, string name, string image, string phone, string address);

        //Lay toan bo Account
        Task<List<AccountTbl>> GetAllAccounts();
        Task<List<AccountTbl>> GetAllAccountsByRole(string role);
        Task<bool> VerifyAccount(string email);

        //Tinh total Account
        Task<int> GetTotalAccounts();
        Task<int> GetTotalAccountsByRole(string role);



        // Update role guest va member.
        Task<bool> UpdateRole(int id, bool check);


        //Cac Interface dung cho Admin
        //Lay toan bo Account


    }
}