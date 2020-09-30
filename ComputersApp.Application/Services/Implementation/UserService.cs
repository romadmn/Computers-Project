using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _unitOfWork.Repository<User>().FindById(userId);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _unitOfWork.Repository<User>().Update(user);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }
    }
}
