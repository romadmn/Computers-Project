using ComputersApp.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface IComputerService
    {
        Task<ComputerDto> GetByIdAsync(int computerId);
        Task<List<ComputerDto>> GetAllAsync();
        Task<bool> UpdateAsync(ComputerDto computer);
        Task<bool> RemoveAsync(int computerId);
        Task<ComputerDto> AddAsync(ComputerDto computer);
    }
}
