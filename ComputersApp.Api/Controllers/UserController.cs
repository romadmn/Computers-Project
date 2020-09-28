using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputersApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ITokenService _tokenService;

        public UserController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenDto>> LoginAsync([FromBody] LoginDto loginDto)
        {
            var user = await _tokenService.VerifyUserCredentialsAsync(loginDto);

            var claims = new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var jwt = _tokenService.GenerateJWT(claims);

            UserTokenDto userTokenDto = new UserTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = new TokenDto() { JWT = jwt }
            };

            return Ok(userTokenDto);
        }

    }
}
