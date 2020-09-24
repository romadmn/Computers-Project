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

        public async Task<CpuDto> GetById(int cpuId)
        {
            return _mapper.Map<CpuDto>(_unitOfWork.Repository<Cpu>().FindById(cpuId));
        }

        public async Task<List<CpuDto>> GetAll()
        {
            return _mapper.Map<List<CpuDto>>(_unitOfWork.Repository<Cpu>().Find().ToList());
        }

        public async Task<bool> Update(CpuDto cpuDto)
        {
            var cpu = _mapper.Map<Cpu>(cpuDto);
            _unitOfWork.Repository<Cpu>().Update(cpu);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Remove(int cpuId)
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

        public async Task<CpuDto> Add(CpuDto cpuDto)
        {
            cpuDto.Id = null;
            var cpu = _mapper.Map<Cpu>(cpuDto);
            await _unitOfWork.Repository<Cpu>().Add(cpu);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CpuDto>(cpu);
        }
    }
}
