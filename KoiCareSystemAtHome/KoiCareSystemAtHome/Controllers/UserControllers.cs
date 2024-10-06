using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly TokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        public UserController(KoiCareSystemDbContext context, TokenProvider tokenProvider, IConfiguration configuration)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModels login)
        {
            var acc = _context.AccountTbls.SingleOrDefault(p => p.Email == login.Email && p.Password == login.Password);
            if (acc == null)
            {
                return BadRequest(new { Success = false, Message = "Email or Password is not correct!" });
            }
            //Tao Token
            TokenModel token = await _tokenProvider.GenerateToken(acc);

            //Neu khong co loi sai thi thuc hien tra ve Token
            return Ok(new
            {
                Success = true,
                Message = "Login Successfully",
                Data = token
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false, //Khong kiem tra han su dung cua Token
            };
            try
            {
                //Checking Token is Valid?
                var tokenInVerification = jwtTokenHandler.ValidateToken(token.AccessToken, tokenValidationParameters,
                    out var validatedToken);
                //Checking Token Algorithms
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result) // In case, result = false
                    {
                        return BadRequest(new { Success = false, Message = "Invalid Token!" });
                    }
                }
                //Checking Expired Token?
                var utcExpireDate = long.Parse(tokenInVerification.Claims
                    .FirstOrDefault(time => time.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate <= DateTime.UtcNow)
                {
                    return BadRequest(new { Success = false, Message = "Token is Expired!" });
                }
                //Checking Token Refresh in Database is Exist?
                var storedToken = _context.RefreshTokens
                    .FirstOrDefault(db => db.Token == token.RefreshToken);
                if (storedToken == null)
                {
                    return BadRequest(new { Success = false, Message = "Token Refresh is not Exist!" });
                }
                //Checking Token is Used? / Revoked?
                if (storedToken.IsUsed == true)
                {
                    return BadRequest(new { Success = false, Message = "Token Refresh has been used!" });
                }
                if (storedToken.IsRevoked == true)
                {
                    return BadRequest(new { Success = false, Message = "Token Refresh has been revoked!" });
                }
                //Checking Id == JwtId? in Refresh Token Db
                var jwtId = tokenInVerification.Claims.FirstOrDefault(token => token.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jwtId)
                {
                    return BadRequest(new { Success = false, Message = "Token is not match" });
                }
                //Update Token is used
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                await _context.SaveChangesAsync();
                //Create new Token
                var acc = await _context.AccountTbls.SingleOrDefaultAsync(acc => acc.AccId == storedToken.AccId);
                TokenModel newToken = await _tokenProvider.GenerateToken(acc);

                return Ok(new { Success = true, Message = "Refresh Token is Success", Data = newToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(string name, string phone, string email, string password, string confirmedPassword)
        {
            if (password != confirmedPassword)
            {
                return BadRequest(new { Susccess = false, Message = "Confirmed password is not correct!" });
            }
            var newAccount = new AccountTbl
            {
                Name = name,
                Phone = phone,
                Email = email,
                Password = password
            };
            await _context.AccountTbls.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Register Successfull" });
        }
    }
}