using ComputersApp.Domain.Entities;
using ComputersApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Infrastructure
{
    public class ComputersContext : DbContext
    {
        public DbSet<Computer> Computer { get; set; }
        public DbSet<Cpu> Cpu { get; set; }

        public ComputersContext(DbContextOptions<ComputersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ComputerConfiguration());
            modelBuilder.ApplyConfiguration(new CpuConfiguration());

        }
    }
}
