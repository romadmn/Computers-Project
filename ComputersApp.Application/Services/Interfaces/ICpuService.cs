using ComputersApp.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface ICpuService
    {
        Task<CpuDto> GetById(int cpuId);
        Task<List<CpuDto>> GetAll();
        Task<bool> Update(CpuDto cpu);
        Task<bool> Remove(int cpuId);
        Task<CpuDto> Add(CpuDto cpu);
    }
}
