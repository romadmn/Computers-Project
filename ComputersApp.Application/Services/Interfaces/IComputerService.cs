using ComputersApp.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface IComputerService
    {
        Task<ComputerDto> GetById(int computerId);
        Task<List<ComputerDto>> GetAll();
        Task<bool> Update(ComputerDto computer);
        Task<bool> Remove(int computerId);
        Task<ComputerDto> Add(ComputerDto computer);
    }
}
