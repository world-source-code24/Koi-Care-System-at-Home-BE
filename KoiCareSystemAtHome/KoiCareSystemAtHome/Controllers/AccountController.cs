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
        public async Task<IActionResult> UpdateProfile(int accId, AccountDTO newUpdate)
        {
            bool updateSuccess = await _accountRepository.UpdateProfile(accId, newUpdate);
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
            if ( accs.IsNullOrEmpty())
            {
                return BadRequest("No accounts available!!");
            }
            return Ok(new {success = true, accs = accs});
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
                    if (!accs.IsNullOrEmpty())
                    {
                        return Ok(new { success = true, accs = accs });
                    }
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
                return Ok(new {success = true, message = "The status is set to " + status});
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
                await _accountRepository.UpdateAsync(acc);
                return Ok(new { success = true, message = "Update successfully!!"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Changing password
        [HttpPut("change-password{accId}")]
        public async Task<IActionResult> ChangePassword(int accId, string password)
        {
            try
            {
                var acc = await _accountRepository.GetByIdAsync(accId);
                if (acc == null)
                {
                    return NotFound("No account available!!");
                }
                acc.Password = password;
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
