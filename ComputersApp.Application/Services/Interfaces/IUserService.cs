using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int userId);
        Task UpdateAsync(User user);
        Task<User> AddAsync(RegisterDto registerDto);
    }
}
