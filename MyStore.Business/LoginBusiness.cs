using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyStore.DataAccess.DTOs;
using MyStore.DataAccess.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreBis
{
    public class LoginBusiness
    {
        private readonly MyStoreContext _context;

        public LoginBusiness(MyStoreContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseDto> AuthenticateUserAsync(UserLoginDto dto)
        {
            // البحث عن المستخدم + اسم الرول
            var user = await _context.Users

                .Select(u => new
                {
                    User = u,
                    RoleName = u.Role.RoleName
                })

                .FirstOrDefaultAsync(u => u.User.Email == dto.Email);

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            // التحقق من كلمة المرور
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.User.Password
            );

            if (!isValidPassword)
            {
                throw new Exception("Invalid credentials");
            }

            // Claims
            var claims = new[]
            {
new Claim(ClaimTypes.NameIdentifier, user.User.UserId.ToString()),

        new Claim(ClaimTypes.Email, user.User.Email),

        new Claim(ClaimTypes.Name, user.User.UserName),

        new Claim(ClaimTypes.Role, user.RoleName)
    };

            // JWT
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456_MYSTORE_PROJECT")
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: "MyStoreApi",
                audience: "MyStoreUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),

                Username = user.User.UserName
            };
        }


















    }
}