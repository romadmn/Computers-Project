using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Domain.Entities
{
    public class Computer : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Cpu Cpu { get; set; }
        public int? CpuId { get; set; }
        public OsType OsType { get; set; }
        public double RamAmount { get; set; }
        public double SsdAmount { get; set; }

    }
}
