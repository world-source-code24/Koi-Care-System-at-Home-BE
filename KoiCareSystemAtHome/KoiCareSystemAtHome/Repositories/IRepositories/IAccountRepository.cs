using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        //Cac Interface dung cho profile cua user hien tai
        //Get Profile Account by id
        Task<AccountDTO> GetAccountProfile(int id);

        //Update Profile
        Task<bool> UpdateProfile(int id, AccountDTO updateInformation);

        //Cac Interface dung cho Admin
        //Lay toan bo Account
    }
}