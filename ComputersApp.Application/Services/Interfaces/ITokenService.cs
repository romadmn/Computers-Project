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
        Task<User> VerifyUserCredentialsAsync(LoginDto loginDto);
        string GenerateJWT(IEnumerable<Claim> claims);
    }
}
