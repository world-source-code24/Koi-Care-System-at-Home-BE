using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //A little similiar to login so pause it

        // Google sign-in process
        //[HttpGet("register-google")]
        //public IActionResult SignInGoogle()
        //{
        //    var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") };
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}

        //[HttpGet("google-callback")]
        //public async Task<IActionResult> GoogleCallback()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new { message = "Google authentication failed" });
        //    }

        //    // Checking email
        //    var email = result.Principal.FindFirstValue(ClaimTypes.Email);
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return BadRequest(new { message = "Email not found" });
        //    }

        //    // Check if user already exists
        //    var user = await _userRepository.GetUserByEmailAsync(email);
        //    if (user != null)
        //    {
        //        return BadRequest(new { message = "Account already exists", email = user.Email });
        //    }
        //    return Ok(new { message = "Register successful", email = email });
        //}


        //Register by form
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (registerDto == null || string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
            {
                return BadRequest("Invalid registration data.");
            }

            var (user, isUserNew) = await _userRepository.RegisterUserByEmailAsync(registerDto.Email);

            if (isUserNew)
            {
                // You can save the new user in the database here
                var newUser = new UserTbl
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    Phone = registerDto.PhoneNumber,
                    FullName = registerDto.FullName
                };

                // Save the new user (implement SaveUserAsync in IUserRepository)
                await _userRepository.SaveUserAsync(newUser);

                return Ok(new {success=true, message = "Registion successful", email = registerDto.Email });
            }
            else
            {
                return BadRequest(new {success=false, message = "User already exists", email = registerDto.Email });
            }
        }
    }
}
