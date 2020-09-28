using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
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
            return _mapper.Map<ComputerDto>(_unitOfWork.Repository<Computer>().Find(new ComputersWithCpuSpecification(computerId)).FirstOrDefault());
        }

        public async Task<List<ComputerDto>> GetAllAsync()
        {
            return _mapper.Map<List<ComputerDto>>(_unitOfWork.Repository<Computer>().Find(new ComputersWithCpuSpecification()).ToList());
        }

        public async Task<bool> UpdateAsync(ComputerDto computerDto)
        {
            var computer = _mapper.Map<Computer>(computerDto);
            _unitOfWork.Repository<Computer>().Update(computer);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> RemoveAsync(int computerId)
        {
            var computer = await _unitOfWork.Repository<Computer>().FindById(computerId);
            if (computer == null)
            {
                return false;
            }
            _unitOfWork.Repository<Computer>().Remove(computer);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<ComputerDto> AddAsync(ComputerDto computerDto)
        {
            computerDto.Id = null;
            var computer = _mapper.Map<Computer>(computerDto);
            await _unitOfWork.Repository<Computer>().Add(computer);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ComputerDto>(computer);
        }
    }
}
