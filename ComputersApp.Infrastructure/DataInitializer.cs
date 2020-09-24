using ComputersApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputersApp.Infrastructure
{
    public static class DataInitializer
    {
        public static void Initialize(ComputersContext context)
        {
            if (!context.Cpu.Any())
            {
                context.Cpu.AddRange(
                    new Cpu
                    {
                        Name = "Ukrainian",
                        CorsAmount = 4,
                        Frequency = 3600,
                    }
                );
                context.SaveChanges();
            }
            if (!context.Computer.Any())
            {
                context.Computer.AddRange(
                    new Computer
                    {
                        Name = "Hp",
                        CpuId = 1,
                        OsType = OsType.Windows,
                        RamAmount = 4000,
                        SsdAmount = 256
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
