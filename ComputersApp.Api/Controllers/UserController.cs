using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Exceptions;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain.Entities;
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

        // GET: api/Computer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAsync([FromRoute] int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<User>> PostAsync([FromBody] RegisterDto registerDto)
        {
            var user = await _userService.AddAsync(registerDto);
            return CreatedAtAction("GetAsync", new { id = user.Id }, user);
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<UserTokenDto>> AuthenticateAsync([FromBody] LoginDto loginDto)
        {
            var user = _tokenService.VerifyUserCredentialsAsync(loginDto);
            
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
        public async Task<ActionResult<UserTokenDto>> RefreshAsync(TokenDto tokenDto)
        {
            string accessToken = tokenDto.JWT;
            string refreshToken = tokenDto.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            int userId;
            int.TryParse(principal.FindFirst("id")?.Value, out userId);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new BadRequestException("Invalid client request");
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
        public async Task<IActionResult> RevokeAsync(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            int userId;
            int.TryParse(principal.FindFirst("id")?.Value, out userId);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userService.UpdateAsync(user);
            return NoContent();
        }

    }
}
