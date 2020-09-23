using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Application.Services.Interfaces;
using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Implementation
{
    public class ComputerService : IComputerService
    {
        private readonly IRepository<Computer> _computerRepository;
        private readonly IMapper _mapper;
        public ComputerService(IRepository<Computer> computerRepository, IMapper mapper)
        {
            _computerRepository = computerRepository;
            _mapper = mapper;
        }

        public async Task<ComputerDto> GetById(int computerId)
        {
            return _mapper.Map<ComputerDto>(await _computerRepository.GetAll().Include(p => p.Cpu).FirstOrDefaultAsync(p => p.Id == computerId));
        }

        public async Task<List<ComputerDto>> GetAll()
        {
            return _mapper.Map<List<ComputerDto>>(await _computerRepository.GetAll().Include(x=>x.Cpu).ToListAsync());
        }

        public async Task<bool> Update(ComputerDto computerDto)
        {
            var computer = _mapper.Map<Computer>(computerDto);
            _computerRepository.Update(computer);
            var affectedRows = await _computerRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> Remove(int computerId)
        {
            var computer = await _computerRepository.FindByCondition(x=>x.Id == computerId);
            if (computer == null)
            {
                return false;
            }
            _computerRepository.Remove(computer);
            var affectedRows = await _computerRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<ComputerDto> Add(ComputerDto computerDto)
        {
            computerDto.Id = null;
            var computer = _mapper.Map<Computer>(computerDto);
            await _computerRepository.Add(computer);
            await _computerRepository.SaveChangesAsync();
            return _mapper.Map<ComputerDto>(computer);
        }
    }
}
