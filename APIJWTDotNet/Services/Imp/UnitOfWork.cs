using APIJWTDotNet.Helpers;
using APIJWTDotNet.Models;
using APIJWTDotNet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJWTDotNet.Services.Imp
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IOptions<JWTHelper> _jwt;


        public UnitOfWork(UserManager<ApplicationUser> manager, IOptions<JWTHelper> jwt)
        {
            _manager = manager;
            _jwt = jwt;
        }
        public IAuthService auth { get { return new AuthService(_manager, _jwt); } }
    }
}
