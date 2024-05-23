using APIJWTDotNet.Helpers;
using APIJWTDotNet.Models;
using APIJWTDotNet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIJWTDotNet.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly JWTHelper _jwt;

        public AuthService(UserManager<ApplicationUser> manager , IOptions<JWTHelper>jwt)
        {
            _manager = manager;
            _jwt = jwt.Value;

        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();
            var user = await _manager.FindByEmailAsync(model.Email);
            if (user is null || !(await _manager.CheckPasswordAsync(user, model.Password)))
            {
                authModel.Message = "Email or password is incorrect";
                return authModel;
            }
            var token = await GenerateTokenAsync(user);
            authModel.IsAuthenticated = true;
            authModel.Exipred = token.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authModel.Username = user.UserName;
            authModel.Email = user.Email;
            var roles = await _manager.GetRolesAsync(user);
            authModel.Roles = new List<string>();
            foreach(var role in roles)
            {
                authModel.Roles.Add(role);
            }

            return authModel;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel register)
        {
            if(await _manager.FindByEmailAsync(register.Email) is not null)
            {
                return new AuthModel() { Message = "Email is already exist" };
            }
            if (await _manager.FindByNameAsync(register.Username) is not null)
            {
                return new AuthModel() { Message = "Username is already exist" };
            }
            var user = new ApplicationUser()
            {
                UserName = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                
                
            };
            var res = await _manager.CreateAsync(user,register.Password);
            if (!res.Succeeded)
            {
                string errors = string.Empty;
                foreach(var error in res.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthModel { Message = errors };
            }
            await _manager.AddToRoleAsync(user, "User");
            var jwtToken = await GenerateTokenAsync(user);
            return new AuthModel
            {
                Email = user.Email,
                Username = user.UserName,
                IsAuthenticated = true,
                Roles = new List<string>() { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),

                Exipred = jwtToken.ValidTo
            };
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {

            var userClaim = await _manager.GetClaimsAsync(user);
            var roles = await _manager.GetRolesAsync(user);
            var roleClaims= new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }


            var claims = new Claim[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Email,user.Email),
                 new Claim("uid",user.Id)
            }.Union(userClaim)
            .Union(roleClaims);

            var symitricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var signingCredintiels = new SigningCredentials(symitricSecurityKey, SecurityAlgorithms.HmacSha256
                );

            var jwtSecurity = new JwtSecurityToken(
                issuer :_jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.Expired),
                signingCredentials: signingCredintiels


            );

           
            return jwtSecurity;

        }
    }
}
