using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hastnama.Solico.Application.Common.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ISolicoDbContext _context;
        private readonly JwtSetting _jwtSettings;
        private readonly IRequestMeta _requestMeta;


        public TokenGenerator(IRequestMeta requestMeta, IOptions<JwtSetting> jwtSettings, ISolicoDbContext context)
        {
            _requestMeta = requestMeta;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        public async Task<AuthResult> Generate(User user, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new("fullName", $"{user.Name} {user.Family}"),
                new("Id", user.Id.ToString()),
                new("RandomId", new Random().Next(1, 10000).ToString()),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim("Email", $"{user.Email}"));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.ExpireDays),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtSettings.ValidAudience,
                Issuer = _jwtSettings.ValidIssuer,
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(securityToken);

            await _context.UserTokens.AddAsync(new UserToken
            {
                CreateDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddDays(_jwtSettings.ExpireDays),
                Token = token,
                Browser = _requestMeta.Browser,
                UserAgent = _requestMeta.UserAgent,
                Ip = _requestMeta.Ip,
                UserId = user.Id,
                IsUsed = false,
            }, cancellationToken);


            await _context.SaveAsync(cancellationToken);

            return new AuthResult
            {
                Token = token
            };
        }


        public async Task<AuthResult> GenerateCustomerToken(Customer customer, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new("fullName", $"{customer.FullName}"),
                new("Id", customer.Id.ToString()),
                new("CustomerId", customer.SolicoCustomerId),
                new("RandomId", new Random().Next(1, 10000).ToString()),
                new("Mobile", customer.Mobile)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.ExpireDays),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtSettings.ValidAudience,
                Issuer = _jwtSettings.ValidIssuer,
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(securityToken);

            await _context.UserTokens.AddAsync(new UserToken
            {
                CreateDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddDays(_jwtSettings.ExpireDays),
                Token = token,
                Browser = _requestMeta.Browser,
                UserAgent = _requestMeta.UserAgent,
                Ip = _requestMeta.Ip,
                CustomerId = customer.Id,
                IsUsed = false,
            }, cancellationToken);


            await _context.SaveAsync(cancellationToken);

            return new AuthResult
            {
                Token = token
            };
        }
    }
}