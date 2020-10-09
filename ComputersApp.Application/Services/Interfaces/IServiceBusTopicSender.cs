using ComputersApp.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Application.Services.Interfaces
{
    public interface IServiceBusTopicSender
    {
        Task SendMessage(ComputerDto computerDto);
    }
}
