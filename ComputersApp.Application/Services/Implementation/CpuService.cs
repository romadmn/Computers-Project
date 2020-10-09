using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Exceptions;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class CpuService : ICpuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CpuService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CpuDto> GetByIdAsync(int cpuId)
        {
            var cpu = _mapper.Map<CpuDto>(await _unitOfWork.Repository<Cpu>().FindById(cpuId));
            if (cpu == null)
                throw new NotFoundException($"Cpu with id = {cpuId} not found!");
            return cpu;
        }

        public async Task<List<CpuDto>> GetAllAsync()
        {
            var cpus = _mapper.Map<List<CpuDto>>(_unitOfWork.Repository<Cpu>().Find().ToList());
            if (cpus == null)
                throw new NotFoundException("Cpus not found!");
            return cpus;
        }

        public async Task UpdateAsync(CpuDto cpuDto)
        {
            var cpu = _mapper.Map<Cpu>(cpuDto);
            _unitOfWork.Repository<Cpu>().Update(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"Cpu with id = {cpuDto.Id} was not updated!");
        }

        public async Task RemoveAsync(int cpuId)
        {
            var cpu = await _unitOfWork.Repository<Cpu>().FindById(cpuId);
            if (cpu == null)
                throw new NotFoundException($"Cpu with id = {cpuId} not found!");
            _unitOfWork.Repository<Cpu>().Remove(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"Cpu with id = {cpuId} was not deleted!");
        }

        public async Task<CpuDto> AddAsync(CpuDto cpuDto)
        {
            cpuDto.Id = null;
            var cpu = _mapper.Map<Cpu>(cpuDto);
            await _unitOfWork.Repository<Cpu>().Add(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows <= 0)
                throw new BadRequestException($"Cpu was not added!");
            return _mapper.Map<CpuDto>(cpu);
        }
    }
}
