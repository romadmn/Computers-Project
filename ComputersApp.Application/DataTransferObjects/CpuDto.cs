using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.DataTransferObjects
{
    public class CpuDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int CorsAmount { get; set; }
        public double Frequency { get; set; }
        public List<ComputerDto> Computers { get; set; }
    }
}
