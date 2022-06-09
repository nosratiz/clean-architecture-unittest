using Hastnama.Solico.Api;
using Hastnama.Solico.Application.Auth.Command.Login;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Domain.Models.UserManagement;
using Hastnama.Solico.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Hastnama.Solico.IntegrationTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient _httpClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(service =>
                    {
                        service.RemoveAll(typeof(ISolicoDbContext));
                        service.AddDbContext<SolicoContext>
                            (option => { option.UseInMemoryDatabase("TestDb"); });

                        service.AddScoped<ISolicoDbContext>(provider => provider.GetService<SolicoContext>());
                    });
                });

            #region Seed Data
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ISolicoDbContext>();

                if (context != null)
                {
                    if (context.Roles.Any() == false)
                    {
                        context.Roles.Add(new Role
                        {
                            Name = "Admin",
                            Title = "ادمین"
                        });

                        context.SaveAsync(CancellationToken.None);
                    }

                    if (context.Users.Any() == false)
                    {
                        context.Users.Add(new User
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
                        });
                    }
                }
            }
            #endregion

            _httpClient = appFactory.CreateClient();
        }

        protected async Task GetAuthorizationAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetJwtTokenAsync());
        }

        private async Task<string> GetJwtTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("/account/Login", new LoginCommand
            {
                UserName = "09107602786",
                Password = "nima1234!"
            });

            var loginResponse = await response.Content.ReadAsAsync<AuthResult>();

            return loginResponse!.Token;
        }
    }
}