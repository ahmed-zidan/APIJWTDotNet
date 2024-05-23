using APIJWTDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJWTDotNet.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel register);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    }
}
