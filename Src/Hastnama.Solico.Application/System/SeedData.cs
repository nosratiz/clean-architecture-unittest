using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.System
{
    public class SeedData
    {
        private readonly ISolicoDbContext _context;

        public SeedData(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task SeedAllAsync(CancellationToken cancellation)
        {
            #region SeedData


            if (!await _context.Roles.AnyAsync(cancellation))
            {
                await _context.Roles.AddAsync(new Role
                {
                    Name = "Admin",
                    Title = "ادمین"
                }, cancellation);
                await _context.Roles.AddAsync(new Role
                {
                    Name = "User",
                    Title = "کاربر",
                }, cancellation);

                await _context.SaveAsync(cancellation);

            }

            if (!await _context.Users.AnyAsync(cancellation))
            {
                await _context.Users.AddAsync(new User
                {

                    Name = "نیما",
                    Family = "نصرتی",
                    Email = "nimanosrati93@gmail.com",
                    Password = PasswordManagement.HashPass("nima1234!"),
                    ActivationCode = Guid.NewGuid().ToString("N"),
                    PhoneNumber = "09107602786",
                    IsEmailConfirmed = true,
                    IsPhoneConfirmed = true,
                    IsDeleted = false,
                    RoleId = 1,
                    RegisterDate = DateTime.Now,
                    ExpiredVerification = DateTime.Now.AddDays(2)
                }, cancellation);

                await _context.SaveAsync(cancellation);
            }

            if (!await _context.ContactUs.AnyAsync(cancellation))
            {
                await _context.ContactUs.AddAsync(new ContactUs
                {
                    Name = "عادل",
                    Email = "adelmehraban@gmail.com",
                    Message = "سلام",
                    CreateDate = DateTime.Now
                }, cancellation);
                await _context.SaveAsync(cancellation);
            }

            if (!await _context.AppSettings.AnyAsync(cancellation))
            {
                await _context.AppSettings.AddAsync(new AppSetting
                {
                    Title = "کاله",
                    MaxSlideShow = 5,
                    MaxSizeUploadFile = 2000,
                    UserId = 1

                }, cancellation);

                await _context.SaveAsync(cancellation);
            }

            
            #endregion

        }
    }
}
