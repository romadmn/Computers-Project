using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
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
            return _mapper.Map<CpuDto>(await _unitOfWork.Repository<Cpu>().FindById(cpuId));
        }

        public async Task<List<CpuDto>> GetAllAsync()
        {
            return _mapper.Map<List<CpuDto>>(_unitOfWork.Repository<Cpu>().Find().ToList());
        }

        public async Task<bool> UpdateAsync(CpuDto cpuDto)
        {
            var cpu = _mapper.Map<Cpu>(cpuDto);
            _unitOfWork.Repository<Cpu>().Update(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> RemoveAsync(int cpuId)
        {
            var cpu = await _unitOfWork.Repository<Cpu>().FindById(cpuId);
            if (cpu == null)
            {
                return false;
            }
            _unitOfWork.Repository<Cpu>().Remove(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<CpuDto> AddAsync(CpuDto cpuDto)
        {
            cpuDto.Id = null;
            var cpu = _mapper.Map<Cpu>(cpuDto);
            await _unitOfWork.Repository<Cpu>().Add(cpu);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CpuDto>(cpu);
        }
    }
}
