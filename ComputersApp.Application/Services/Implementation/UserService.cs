using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using ComputersApp.Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<User> AddAsync(RegisterDto registerDto)
        {
            var userWithTheSameEmail = _unitOfWork.Repository<User>().Find(new LoginSpecification(registerDto.Email)).SingleOrDefault();
            if (userWithTheSameEmail != null)
            {
                return null;
            }
            var newUser = _mapper.Map<User>(registerDto);
            await _unitOfWork.Repository<User>().Add(newUser);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0 ? newUser : null;
        }
    }
}
