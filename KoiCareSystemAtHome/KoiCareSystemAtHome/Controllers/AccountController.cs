using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly IAccountRepository _accountRepository;

        public AccountController(KoiCareSystemDbContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        //Profile cua User dang su dung
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var ClaimAccId = User.FindFirst("Id")?.Value;

            if (ClaimAccId == null || !int.TryParse(ClaimAccId, out int accId))
            {
                return Unauthorized("ID không hợp lệ trong token");
            }

            AccountDTO profile = await _accountRepository.GetAccountProfile(accId);
            if (profile == null)
            {
                return NotFound(new { message = "Don't have any information of this account!" });
            }
            return Ok(profile);
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(int accId, string name, string image, string phone, string address)
        {
            bool updateSuccess = await _accountRepository.UpdateProfile(accId, name, image, phone, address);
            if (!updateSuccess)
            {
                return BadRequest(new { success = false, message = "Cannot update Profile" });
            }
            return Ok(new { success = true, message = "Update profile is successful!" });
        }

        //Get all account except admin
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accs = await _accountRepository.GetAllAccounts();

            int totalAccounts = await _accountRepository.GetTotalAccounts();
            return Ok(new { success = true, accs = accs, total = totalAccounts });

        }

        //Get all by role
        [HttpGet("get-all-by{role}")]
        public async Task<IActionResult> GetAllByRole(string role)
        {
            try
            {
                if (role != null)
                {
                    var accs = await _accountRepository.GetAllAccountsByRole(role);
                    int totalAccounts = await _accountRepository.GetTotalAccountsByRole(role);
                    return Ok(new { accList = accs, total = totalAccounts });
                }
                return BadRequest("No accounts available!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Edit status 
        //Delete account means setting status to false
        [HttpPut("edit-status{accId}")]
        public async Task<IActionResult> EditStatus(int accId, bool status)
        {
            try
            {
                var acc = await _accountRepository.GetByIdAsync(accId);
                if (acc == null)
                {
                    return NotFound("No account available!!");
                }
                acc.Status = status;
                await _accountRepository.UpdateAsync(acc);
                return Ok(new { success = true, message = "The status is set to " + status });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Access membership
        [HttpPut("membership{accId}")]
        public async Task<IActionResult> MembershipAccess(int accId)
        {
            try
            {
                var acc = await _accountRepository.GetByIdAsync(accId);
                if (acc == null)
                {
                    return NotFound("No account available!!");
                }
                acc.Role = "member";
                acc.StartDate = DateOnly.FromDateTime(DateTime.Now);
                var cartTbl = new CartTbl
                {
                    ProductId = 1002,
                    AccId = accId,
                    Quantity = 1
                };
                _context.CartTbls.Add(cartTbl);
                await _accountRepository.UpdateAsync(acc);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Update successfully!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Changing password
        [HttpPut("change-password{accId}")]
        public async Task<IActionResult> ChangePassword(int accId, string changePassword, string confirmedPassword)
        {
            try
            {
                var acc = await _accountRepository.GetByIdAsync(accId);
                if (acc == null)
                {
                    return NotFound("No account available!!");
                }
                if (acc.Password == confirmedPassword)
                {
                    acc.Password = changePassword;
                }
                await _accountRepository.UpdateAsync(acc);
                return Ok(new { success = true, message = "Changing successfully!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
