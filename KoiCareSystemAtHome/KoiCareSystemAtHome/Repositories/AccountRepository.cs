using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly KoiCareSystemDbContext _context;
        public AccountRepository(KoiCareSystemDbContext context)
        {
            _context = context;
        }
        public async Task<AccountDTO> GetAccountProfile(int id)
        {
            //Get information of Account in Db
            var account = await _context.AccountTbls.FirstOrDefaultAsync(acc => acc.AccId == id);
            //Check account is exist?
            if (account == null)
            {
                return null;
            }
            //Put information in to account Dto and put it in to controller layer
            var accountDto = new AccountDTO
            {
                AccId = account.AccId,
                Image = account.Image,
                Name = account.Name,
                Email = account.Email,
                Phone = account.Phone,
                Role = account.Role,
            };
            return accountDto;
        }

        public async Task<bool> UpdateProfile(int id, AccountDTO updateInformation)
        {
            var account = _context.AccountTbls.FirstOrDefault(acc => acc.AccId == id);
            if (account == null)
            {
                return false;
            }
            if (updateInformation == null)
            {
                return false;
            }
            account.Image = updateInformation.Image;
            account.Name = updateInformation.Name;
            account.Address = updateInformation.Address;
            account.Phone = updateInformation.Phone;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}