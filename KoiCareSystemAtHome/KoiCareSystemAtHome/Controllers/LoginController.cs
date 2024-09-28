using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using KoiCareSystemAtHome.Repositories.IRepository;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;

        private readonly IUserRepository _userRepository;

        public LoginController(KoiCareSystemDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        //Login with Google
        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Google authentication failed" });
            }

            //Checking email | Not Exist => Email not found
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email not found" });
            }

            //Get user to sign in
            var user = await _userRepository.GetUserByEmailAsync(email);

            await SignInUser(user);
            return Redirect("http://localhost:5173/");
    }

        private async Task SignInUser(UserTbl user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            // Login with cookie authentication
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        //Login with email of System (email that register with database)
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid Request");
            }

            var user = await _context.UserTbls.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || user.Password != loginDto.Password)
            {
                return Unauthorized("Don't have user or incorrect password");
            }
            return Ok(new {success=true, Message = "Login Successfully" });
        }
    }
}
