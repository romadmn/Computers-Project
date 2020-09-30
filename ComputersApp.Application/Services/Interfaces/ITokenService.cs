using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface ITokenService
    {
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public User VerifyUserCredentialsAsync(LoginDto loginDto);
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
