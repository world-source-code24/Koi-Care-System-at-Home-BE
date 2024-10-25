using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class AccountRepository : GenericRepository<AccountTbl>, IAccountRepository
    {
        //private readonly IAccountRepository _accountRepository;
        private readonly KoicareathomeContext _context;

        public AccountRepository (KoicareathomeContext context) : base(context)
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
                Address = account.Address,
                Role = account.Role,
            };
            return accountDto;
        }
        public async Task<List<AccountTbl>> GetAllAccounts()
        {
            return await _context.AccountTbls.ToListAsync();
        }
        public async Task<List<AccountTbl>> GetAllAccountsByRole(string role)
        {
            return await _context.AccountTbls.Where(a => a.Role.ToLower().Equals(role.ToLower())).ToListAsync();
        }
        public async Task<bool> UpdateProfile(int id, string name, string image, string phone, string address)
        {
            var account = _context.AccountTbls.FirstOrDefault(acc => acc.AccId == id);
            if (account == null)
            {
                return false;
            }if (image == null) image = "";
            account.Image = image;
            account.Name = name;
            account.Address = address;
            account.Phone = phone;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateRole(int id, bool check)
        {
            var getProfile =await GetAccountProfile(id);
            if (check)
            {
                getProfile.Role = AllEnum.UserRole.Member.ToString();
                return true;
            }
            else
            {
               getProfile.Role= AllEnum.UserRole.Guest.ToString();
                return false;
            }

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
        public async Task<int> GetTotalAccounts()
        {return await _context.AccountTbls.CountAsync(acc => acc.Status && !acc.Role.Equals("admin"));
        }
        public async Task<int> GetTotalAccountsByRole(string role)
        {
            return await _context.AccountTbls.CountAsync(acc => acc.Status && acc.Role.Equals(role));
        }
    }
}
