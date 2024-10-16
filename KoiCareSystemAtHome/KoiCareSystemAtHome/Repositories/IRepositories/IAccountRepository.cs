﻿using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<AccountTbl>
    {
        //Tao tai khoan moi
        Task<bool> RegisterAccount(AccountDTO account);
        //Cac Interface dung cho profile cua user hien tai
        //Get Profile Account by id
        Task<AccountDTO> GetAccountProfile(int id);
        //Update Profile
        Task<bool> UpdateProfile(int id, AccountDTO updateInformation);

        //Cac Interface dung cho Admin
        //Lay toan bo Account
        Task<List<AccountTbl>> GetAllAccounts();
        Task<List<AccountTbl>> GetAllAccountsByRole(string role);
        //Get Account by Email
        Task<bool> VerifyAccount(string email);
    }
}