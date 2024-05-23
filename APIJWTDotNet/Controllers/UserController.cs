using APIJWTDotNet.Models;
using APIJWTDotNet.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJWTDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public UserController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _uof.auth.RegisterAsync(model);
            if (!res.IsAuthenticated)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _uof.auth.GetTokenAsync(model);
            if (!res.IsAuthenticated)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

    }
}
