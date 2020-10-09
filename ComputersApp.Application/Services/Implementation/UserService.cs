using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Exceptions;
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
            var user = await _unitOfWork.Repository<User>().FindById(userId);
            if (user == null)
                throw new NotFoundException($"User with id = {userId} not found!");
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _unitOfWork.Repository<User>().Update(user);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"User with id = {user.Id} was not updated!");
        }

        public async Task<User> AddAsync(RegisterDto registerDto)
        {
            var userWithTheSameEmail = _unitOfWork.Repository<User>().Find(new LoginSpecification(registerDto.Email)).SingleOrDefault();
            if (userWithTheSameEmail != null)
                throw new NotFoundException($"User with Email = {registerDto.Email} already exist!");
            var newUser = _mapper.Map<User>(registerDto);
            await _unitOfWork.Repository<User>().Add(newUser);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"User was not registered!");
            return newUser;
        }
    }
}
