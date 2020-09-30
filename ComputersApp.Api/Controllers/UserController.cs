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
        private IUserService _userService;

        public UserController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenDto>> AuthenticateAsync([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid client request");
            }
            var user = _tokenService.VerifyUserCredentialsAsync(loginDto);
            if (user == null)
            {
                return Unauthorized();
            }
            
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userService.UpdateAsync(user);
            return Ok(new UserTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = new TokenDto() { JWT = accessToken, RefreshToken = refreshToken }
            });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<UserTokenDto>> Refresh(TokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = tokenDto.JWT;
            string refreshToken = tokenDto.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userId = int.Parse(principal.FindFirst("id")?.Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userService.UpdateAsync(user);
            return Ok(new UserTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = new TokenDto() { JWT = newAccessToken, RefreshToken = newRefreshToken }
            });
        }
        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var userId = int.Parse(principal.FindFirst("id")?.Value);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userService.UpdateAsync(user);
            return NoContent();
        }

    }
}
