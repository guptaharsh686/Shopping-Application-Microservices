﻿using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> register()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> login()
        {
            return Ok();
        }
    }
}