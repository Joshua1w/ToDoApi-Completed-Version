using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TodoApi.Helpers
{
    public class JwtService
    {
        private readonly string _key;

        // Constructor to initialize the key
        public JwtService(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length < 64)
            {
                throw new ArgumentException("JWT signing key must be at least 64 characters long.");
            }

            _key = key;
        }

        // Method to create a JWT token
        public string CreateToken(int userId, string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),  // Set token expiration (1 day in this case)
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Generate token
        }
    }
}