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
        private readonly KoicareathomeContext _context;
        public TokenProvider(IConfiguration configuration, KoicareathomeContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<TokenModel> GenerateToken(AccountTbl account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            // Create claims with unique identifiers
            var claims = new[]
            {
        new Claim("Id", account.AccId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, account.Email),
        new Claim(ClaimTypes.Role, account.Role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier
    };

            // Log claims for debugging
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            // Set the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10), // Set expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };

            // Create and write the token
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            // Save the refresh token to the database
            var refreshTokenEntity = new RefreshToken
            {
                TokenId = Guid.NewGuid().ToString(),
                AccId = account.AccId,
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssueAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(30), // Set a longer expiry for the refresh token
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity); // Use the correct DbSet
            await _context.SaveChangesAsync();

            // Return the token model
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