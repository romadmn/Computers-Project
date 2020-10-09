using ComputersApp.CosmoDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Models
{
    public class ComputersDbContext : DbContext
    {
        private string _endPoint;
        private string _accountKey;
        private string _databaseName;
        public DbSet<Computer> Computers { get; set; }

        public ComputersDbContext(string endPoint, string accountKey, string databaseName)
        {
            _endPoint = endPoint;
            _accountKey = accountKey;
            _databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(_endPoint, _accountKey, _databaseName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Computers");
            modelBuilder.Entity<Computer>().OwnsOne(e => e.Cpu);
        }
    }
}
