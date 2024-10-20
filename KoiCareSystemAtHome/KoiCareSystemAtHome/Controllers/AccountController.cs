 using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            //Get Id of Account in Token
            var accIdClaim = User.FindFirst("Id")?.Value;
            //try to tranfer idClaim to int account
            if (accIdClaim == null || !int.TryParse(accIdClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
            AccountDTO profile = await _accountRepository.GetAccountProfile(accId);
            if (profile == null)
            {
                return NotFound(new { message = "Don't have any information of this account!" });
            }
            return Ok(profile);
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(AccountDTO newUpdate)
        {
            //Get Id of Account in Token
            var accIdClaim = User.FindFirst("Id")?.Value;
            //try to tranfer accIdClaim to int account
            if (accIdClaim == null || !int.TryParse(accIdClaim, out int accId))
            {
                return BadRequest("User ID not found or invalid.");
            }
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
            return Ok(new {success = true, accs = accs});
        }

        //Get all by role
        [HttpGet("get-all-by{role}")]
        public async Task<IActionResult> GetAllByRole(string role)
        {
            try
            {


                if (role == null)
                {
                    return BadRequest("Role should by provided!!");
                }
                var accs = await _accountRepository.GetAllAccountsByRole(role);
                return Ok(new { success = true, accs = accs });
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
