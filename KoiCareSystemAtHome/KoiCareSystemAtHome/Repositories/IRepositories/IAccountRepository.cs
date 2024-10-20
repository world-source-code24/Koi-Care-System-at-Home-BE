﻿using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<AccountTbl>
    {
        //Cac Interface dung cho profile cua user hien tai
        //Get Profile Account by id
        Task<AccountDTO> GetAccountProfile(int id);

        //Update Profile
        Task<bool> UpdateProfile(int id, AccountDTO updateInformation);
        Task<List<AccountTbl>> GetAllAccounts();
        Task<List<AccountTbl>> GetAllAccountsByRole(string role);

        // Update role guest va member.
        Task<bool> UpdateRole(int id, bool check);


        //Cac Interface dung cho Admin
        //Lay toan bo Account
    }
}