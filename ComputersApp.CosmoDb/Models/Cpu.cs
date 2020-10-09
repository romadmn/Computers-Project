using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Models
{
    public class Cpu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CorsAmount { get; set; }
        public double Frequency { get; set; }

    }
}
