using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Domain.Entities
{
    public class Cpu : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CorsAmount { get; set; }
        public double Frequency { get; set; }
        public List<Computer> Computers { get; set; }

    }
}
