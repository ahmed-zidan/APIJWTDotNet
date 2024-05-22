using APIJWTDotNet.Helpers;
using APIJWTDotNet.Models;
using APIJWTDotNet.Services.Imp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJWTDotNet.Services.Interfaces
{
    public interface IUnitOfWork
    {
        public IAuthService auth { get; }
    }
}
