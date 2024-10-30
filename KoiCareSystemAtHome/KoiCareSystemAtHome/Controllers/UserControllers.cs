using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MailKit.Net.Smtp;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MimeKit;
using System.Diagnostics;



namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly KoicareathomeContext _context;
        private readonly TokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        public UserController(KoicareathomeContext context, TokenProvider tokenProvider, IConfiguration configuration, IAccountRepository accountRepository)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModels login)
        {
            if (login == null)
            {
                return BadRequest(new { Success = false, Message = "Email and Password can not blank!" });
            }
            var acc = _context.AccountTbls.SingleOrDefault(acc => acc.Email == login.Email && acc.Status);
            if (acc == null)
            {
                return BadRequest(new { Success = false, Message = "Your account not exist!" });
            }
            if (acc.Email != login.Email)
            {
                return BadRequest(new { Success = false, Message = "Wrong Email!" });
            }
            if (acc.Password != login.Password)
            {
                return BadRequest(new { Success = false, Message = "Wrong Password!" });
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

        [HttpPut("RefeshToken")]

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(string name, string phone, string email, string password, string confirmedPassword)
        {
            var account = _context.AccountTbls.SingleOrDefault(acc => acc.Email == email && acc.Status);
            if (account != null)
            {
                return BadRequest(new { Susccess = false, Message = "Account was existed!" });
            }
            if (_context.AccountTbls.SingleOrDefault(acc => acc.Phone == phone  && acc.Status) != null)
            {
                return BadRequest(new { Susccess = false, Message = "Phone was existed!" });
            }
            if (password != confirmedPassword)
            {
                return BadRequest(new { Susccess = false, Message = "Confirmed password is not correct!" });
            }

            var newAccount = new AccountTbl
            {
                Name = name,
                Phone = phone,
                Email = email,
                Password = password,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                Status = false,
            };
            await _accountRepository.AddAsync(newAccount);
            return Ok(new { Success = true, Message = "Account registered successfully. Please check your email to verify your account." });
        }

        [HttpPost("verficode")]
        public async Task<IActionResult> SendEmail(string receiveEmail)
        {
            if (string.IsNullOrWhiteSpace(receiveEmail))
            {
                return BadRequest("Email address is required.");
            }

            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("KoiCare", _configuration["EmailSettings:FromAddress"])); // Use app settings for flexibility
                email.To.Add(new MailboxAddress("Receiver", receiveEmail));
                email.Subject = "Testing out email sending";
                Random random = new Random();
                var code = random.Next(100000, 1000000).ToString();
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = "Use this code to verify email: " + code
                };

                var smtpHost = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

                using (var smtp = new SmtpClient())
                {
                    // Connect to SMTP server and authenticate
                    await smtp.ConnectAsync(smtpHost, smtpPort, false);
                    await smtp.AuthenticateAsync(smtpUser, smtpPassword);

                    // Send the email
                    await smtp.SendAsync(email);

                    // Disconnect safely
                    await smtp.DisconnectAsync(true);
                }

                return Ok(new { success = true, message = "Email sent successfully", code = code });
            }
            catch (Exception ex)
            {
                // Log the exception (consider logging frameworks like Serilog, NLog, etc.)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> verifyAccount(int userCode, int verifyCode, string email)
        {
            var acc = await _context.AccountTbls.OrderByDescending(acc => acc.AccId).FirstOrDefaultAsync(acc => acc.Email == email);
            if (acc != null)
            {
                if (userCode == verifyCode)
                {
                    acc.Status = true;
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true });
                }
            }
            return BadRequest();
        }
    }
}