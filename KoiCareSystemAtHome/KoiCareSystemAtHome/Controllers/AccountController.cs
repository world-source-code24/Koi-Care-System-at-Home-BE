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

        //Phan ma cac Admin se su dung
        //Admin se get toan bo user trong he thong tru user da bi xoa
    }
}
