using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Models
{
    public class Computer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Cpu Cpu { get; set; }
        public int? CpuId { get; set; }
        public OsType OsType { get; set; }
        public double RamAmount { get; set; }
        public double SsdAmount { get; set; }
    }
}
