using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Helpers;
using TodoApi.Data;
using System.Text;

namespace TodoApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new AppException("Username is already taken", 409); // Conflict
            }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: dto.Password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
            {
                throw new AppException("Invalid username or password", 401); // Unauthorized
            }

            var passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: dto.Password,
                salt: Encoding.UTF8.GetBytes(user.Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (user.PasswordHash != passwordHash)
            {
                throw new AppException("Invalid username or password", 401); // Unauthorized
            }

            return _jwtService.CreateToken(user.Id, user.Username);
        }
    }
}
