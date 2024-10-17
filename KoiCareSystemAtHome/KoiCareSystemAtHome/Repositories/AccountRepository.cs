using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Numerics;

namespace KoiCareSystemAtHome.Repositories
{
    public class AccountRepository : GenericRepository<AccountTbl>, IAccountRepository
    {
        //private readonly IAccountRepository _accountRepository;
        private readonly KoiCareSystemDbContext _context;

        public AccountRepository(KoiCareSystemDbContext context) : base(context)
        {
            _context = context;
        }
        //Register Email
        public async Task<bool> RegisterAccount(AccountDTO account)
        {
            if (account == null)
            {
                return false;
            }
            var newAccount = new AccountTbl
            {
                Name = account.Name,
                Phone = account.Phone,
                Email = account.Email,
                Password = account.Password,
            };
            await _context.AccountTbls.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<List<AccountTbl>> GetAllAccounts()
        {
            return await _context.AccountTbls.Where(a => !a.Role.Equals("admin")).ToListAsync();
        }

        public async Task<List<AccountTbl>> GetAllAccountsByRole(string role)
        {
            return await _context.AccountTbls.Where(a => a.Role.Equals(role)).ToListAsync();
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
        public async Task<bool> VerifyAccount(string email)
        {

            if (email == null)
            {
                return false;
            }
            var account = await _context.AccountTbls.FirstOrDefaultAsync(acc => acc.Email == email);
            if (account == null)
            {
                return false;
            }

           account.Status = true;
           await  _context.SaveChangesAsync();

           return true;
        }
    }
}
