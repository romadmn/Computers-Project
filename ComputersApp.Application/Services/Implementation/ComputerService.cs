using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Exceptions;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using ComputersApp.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class ComputerService : IComputerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ComputerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ComputerDto> GetByIdAsync(int computerId)
        {
            var computer = _mapper.Map<ComputerDto>(_unitOfWork.Repository<Computer>().Find(new ComputersWithCpuSpecification(computerId)).FirstOrDefault());
            if (computer == null)
                throw new NotFoundException($"Computer with id = {computerId} not found!");
            return computer;
        }

        public async Task<List<ComputerDto>> GetAllAsync()
        {
            var computers = _mapper.Map<List<ComputerDto>>(_unitOfWork.Repository<Computer>().Find(new ComputersWithCpuSpecification()).ToList());
            if (computers == null)
                throw new NotFoundException("Computers not found!");
            return computers;
        }

        public async Task UpdateAsync(ComputerDto computerDto)
        {
            var computer = _mapper.Map<Computer>(computerDto);
            _unitOfWork.Repository<Computer>().Update(computer);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if(affectedRows <= 0)
                throw new BadRequestException($"Computer with id = {computerDto.Id} was not updated!");
        }

        public async Task RemoveAsync(int computerId)
        {
            var computer = await _unitOfWork.Repository<Computer>().FindById(computerId);
            if (computer == null)
                throw new NotFoundException($"Computers with id = {computerId} not found!");
            _unitOfWork.Repository<Computer>().Remove(computer);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"Computer with id = {computerId} was not deleted!");
        }

        public async Task<ComputerDto> AddAsync(ComputerDto computerDto)
        {
            computerDto.Id = null;
            var computer = _mapper.Map<Computer>(computerDto);
            await _unitOfWork.Repository<Computer>().Add(computer);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"Computer was not added!");
            return _mapper.Map<ComputerDto>(computer);
        }
    }
}
