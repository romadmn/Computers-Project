using ComputersApp.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface ICpuService
    {
        Task<CpuDto> GetByIdAsync(int cpuId);
        Task<List<CpuDto>> GetAllAsync();
        Task UpdateAsync(CpuDto cpu);
        Task RemoveAsync(int cpuId);
        Task<CpuDto> AddAsync(CpuDto cpu);
    }
}
