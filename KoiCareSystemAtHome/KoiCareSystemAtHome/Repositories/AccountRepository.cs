using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace KoiCareSystemAtHome.Repositories
{
    public class AccountRepository : GenericRepository<AccountTbl>, IAccountRepository
    {
        private readonly KoicareathomeContext _context;
        private readonly IConfiguration _configuration;

        public AccountRepository (KoicareathomeContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _configuration = configuration;
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
            try
            {
                var account = await _context.AccountTbls.SingleOrDefaultAsync(a => a.AccId.Equals(accId));
                if (account != null)
                {
                    if (account.Role.ToLower().Equals("guest"))
                    {
                        var membership = new MembershipDashboard
                        {
                            AccId = accId,
                            Money = 99,
                            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                        };
                        account.Role = "member";
                        account.StartDate = DateOnly.FromDateTime(DateTime.Now);
                        account.EndDate = account.StartDate.AddMonths(6);
                        _context.AccountTbls.Update(account);
                        _context.MembershipDashboards.Add(membership);
                        _context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            //var membership = await _context.MembershipDashboards.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
            //if (membership == null)
            //{
            //    membership = new MembershipDashboard
            //    {
            //        AccId = accId,
            //        Money = 99,
            //        StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            //    };

            //    var accountUpdate = await _context.AccountTbls.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
            //    if (accountUpdate != null) // Ensure accountUpdate is not null
            //    {
            //        accountUpdate.Role = "member"; // Update the role directly
            //        _context.AccountTbls.Update(accountUpdate);
            //        accountUpdate.StartDate = DateOnly.FromDateTime(DateTime.Now);
            //        accountUpdate.EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(6);
            //    }

            //    _context.MembershipDashboards.Add(membership);
            //    await _context.SaveChangesAsync();
            //    return true;
            //}
            //else if (membership != null)
            //{
            //    var accountUpdate = await _context.AccountTbls.FirstOrDefaultAsync(m => m.AccId.Equals(accId));
            //    if (accountUpdate.StartDate >= accountUpdate.EndDate)
            //    {
            //        accountUpdate.Role = "member";
            //        _context.AccountTbls.Update(accountUpdate);
            //        _context.SaveChanges();
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
        }

        public async Task UpdateAndCheckAllUserRole()
        {
            var expiredMemberships = _context.AccountTbls.Where(acc => acc.Role.Equals("member", StringComparison.OrdinalIgnoreCase)
                && acc.EndDate <= DateOnly.FromDateTime(DateTime.Now)).ToList();
            foreach (var account in expiredMemberships)
            {
                account.Role = "guest";
            }
            await _context.SaveChangesAsync();
        }
        private async Task SendExpirationEmailAsync(string emailAddress)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("KoiCare", _configuration["EmailSettings:FromAddress"]));
                email.To.Add(new MailboxAddress("User", emailAddress));
                email.Subject = "Membership Expired";

                email.Body = new TextPart("html")
                {
                    Text = $"<h2>Your Membership Has Expired</h2>" +
                           $"<p>Dear user,</p>" +
                           $"<p>Your membership has expired. If you wish to renew your membership, please log in to your account and select a membership package.</p>"
                };

                var smtpHost = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(smtpHost, smtpPort, false);
                    await smtp.AuthenticateAsync(smtpUser, smtpPassword);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                Console.WriteLine($"Expiration email sent to {emailAddress}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send expiration email to {emailAddress}. Error: {ex.Message}");
            }
        }
    }
}
