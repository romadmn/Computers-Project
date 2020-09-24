using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Infrastructure.Specifications
{
    public class ComputersWithCpuSpecification : BaseSpecification<Computer>
    {
        public ComputersWithCpuSpecification() : base()
        {
            AddInclude(x => x.Cpu);
            AddInclude($"{nameof(Computer.Cpu)}");
        }

        public ComputersWithCpuSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Cpu);
            AddInclude($"{nameof(Computer.Cpu)}");
        }
    }
}
