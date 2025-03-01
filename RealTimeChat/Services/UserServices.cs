using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;
using RealTimeChat.Models.Dtos;

namespace RealTimeChat.Services
{
    public class UserServices
    {
        private readonly AppDbContext _dbContext;
        public UserServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> AddUser(RegistrationDto registrationDto)
        {
            if (await UsernameExistsAsync(registrationDto.Username))
            {
                return false;
            }
            else
            {
                var userEntity = new User()
                {
                    Username = registrationDto.Username,
                    PasswordHash = HashPassword(registrationDto.PasswordHash)
                };
                _dbContext.Add(userEntity);
                _dbContext.SaveChanges();
                return true;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool LoginValid(LoginDto loginDto)
        {
            if (!string.IsNullOrEmpty(loginDto.Username) && !string.IsNullOrEmpty(loginDto.Password))
                return true;
            return false;
        }

        public async Task<User> UserValid(LoginDto loginDto)
        {
            if (LoginValid(loginDto))
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
                if (user == null)
                {
                    return null;
                }
                else if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return user;
                }
            }

            return null;
        }

    }
}
