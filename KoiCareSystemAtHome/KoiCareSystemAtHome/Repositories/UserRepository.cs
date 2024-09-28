using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KoiCareSystemAtHome.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KoiCareSystemDbContext _context;

        public UserRepository(KoiCareSystemDbContext context)
        {
            _context = context;
        }

        //Create New Password | If the first time login
        public string CreateRandomPassword()
        {
            int length = new Random().Next(6, 14);
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        //Get User By Email | If don't find user in Db => create new
        public async Task<UserTbl> GetUserByEmailAsync(string email)
        {
            var user = await _context.UserTbls.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new UserTbl
                {
                    Email = email,
                    Password = CreateRandomPassword()
                };
                _context.UserTbls.Add(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }



        public async Task<(UserTbl, bool)> RegisterUserByEmailAsync(string email)
        {
            //Check path email
            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email format");
            }
            var user = await _context.UserTbls.FirstOrDefaultAsync(u => u.Email == email);
            bool isNewUser = false;
            if (user == null)
            {
                isNewUser = true;
                user = new UserTbl
                {
                    Email = email,
                    Password = CreateRandomPassword()
                };
            }
            return (user, isNewUser);
        }



        public async Task SaveUserAsync(UserTbl user)
        {
            _context.UserTbls.Add(user);
            await _context.SaveChangesAsync();
        }

        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User is not authenticated or user ID is invalid.");
        }

        public Task<bool> CheckPhoneNumber(string phoneNumber)
        {
            bool isMatch = Regex.IsMatch(phoneNumber, @"^?\+?[0-9]{9,11}$");
            return Task.FromResult(isMatch);
        }
    }
}
