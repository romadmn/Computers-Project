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
        Task<bool> UpdateAsync(User user);
    }
}
