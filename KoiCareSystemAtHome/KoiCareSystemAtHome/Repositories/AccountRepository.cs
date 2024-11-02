using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class AccountRepository : GenericRepository<AccountTbl>, IAccountRepository
    {
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
            return await _context.AccountTbls
            .GroupBy(acc => acc.Email)
            .Select(group => group
            .OrderByDescending(acc => acc.AccId)
            .FirstOrDefault())
            .ToListAsync();
        }
        public async Task<List<AccountTbl>> GetAllAccountsByRole(string role)
        {
            return await _context.AccountTbls.Where(a => a.Role.ToLower()
                .Equals(role.ToLower()))
                .GroupBy(acc => acc.Email)
                .Select(group => group
                .OrderByDescending(acc => acc.AccId) 
                .FirstOrDefault())
                .ToListAsync();
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
            var getProfile = await GetAccountProfile(id);
            if (check)
            {
                getProfile.Role = AllEnum.UserRole.Member.ToString();
                return true;
            }
            else
            {
                getProfile.Role = AllEnum.UserRole.Guest.ToString();
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
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<int> GetTotalAccounts()
        {
            return await _context.AccountTbls.CountAsync(acc => acc.Status && !acc.Role.Equals("admin"));
        }
        public async Task<int> GetTotalAccountsByRole(string role)
        {
            return await _context.AccountTbls.CountAsync(acc => acc.Status && acc.Role.Equals(role));
        }

        public async Task<bool> BuyMembership(int accId)
        {
            var membership = await _context.MembershipDashboards.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
            if (membership == null)
            {
                membership = new MembershipDashboard
                {
                    AccId = accId,
                    Money = 99,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                };

                var accountUpdate = await _context.AccountTbls.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
                if (accountUpdate != null) // Ensure accountUpdate is not null
                {
                    accountUpdate.Role = "member"; // Update the role directly
                    accountUpdate.StartDate = DateOnly.FromDateTime(DateTime.Now);
                    accountUpdate.EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(6);
                }

                _context.MembershipDashboards.Add(membership);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (membership != null)
            {
                var accountUpdate = await _context.AccountTbls.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
                if (accountUpdate.StartDate >= accountUpdate.EndDate)
                {
                    accountUpdate.Role = "member";
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
