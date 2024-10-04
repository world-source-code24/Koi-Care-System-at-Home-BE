using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KoiCareSystemAtHome.Repositories
{
    public class TokenProvider
    {
        private readonly IConfiguration _configuration;
        private readonly KoiCareSystemDbContext _context;
        public TokenProvider(IConfiguration configuration, KoiCareSystemDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<TokenModel> GenerateToken(AccountTbl account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // Nguoi dung
                    new Claim("Id", account.AccId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role),

                    // Token
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDecriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Luu vao database
            var refreshTokenEntity = new RefreshToken
            {
                TokenId = Guid.NewGuid().ToString(),
                AccId = account.AccId,
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(10),
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
