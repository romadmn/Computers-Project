using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.DataTransferObjects
{
    public class ComputerDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public CpuDto Cpu { get; set; }
        public OsType? OsType { get; set; }
        public double RamAmount { get; set; }
        public double SsdAmount { get; set; }
    }
}
