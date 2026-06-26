using Microsoft.EntityFrameworkCore;
using MyStore.DataAccess.DTOs;
using MyStore.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreBis
{
    public class AddUserBusiness
    {





        public readonly MyStoreContext _context;

        public AddUserBusiness(MyStoreContext context)
        {
            _context = context; // نخزن النسخة المحقونة جوه المتغير حقنا
        }

        public async Task<int> InsertUserAsync(UserRegisterDto dto)
        {
            bool isEmailExist = await _context.Users.AnyAsync(u => u.Email == dto.Email);

            if (isEmailExist)
            {
                return -1;
            }

            // 🔐 تشفير الباسورد هنا قبل ما يروح للداتا بيز
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                Password = hashedPassword, // 👈 الحين خزنّا النسخة المشفرة السرية!
                RoleId = 2,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser.UserId;
        }




    }
}
