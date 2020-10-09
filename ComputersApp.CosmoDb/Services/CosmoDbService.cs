using ComputersApp.CosmoDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputersApp.CosmoDb.Services
{
    public class CosmosDbService : ICosmosDbService<Computer, string>
    {
        private readonly IConfiguration _configuration;

        public CosmosDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Computer> CreateAsync(Computer entity)
        {
            using (var computersDbContext =
                new ComputersDbContext(
                    _configuration["CosmosDbSettings:EndPoint"].ToString(),
                    _configuration["CosmosDbSettings:AccountKey"].ToString(),
                    _configuration["CosmosDbSettings:DatabaseName"].ToString()))
            {
                entity.Id = Guid.NewGuid().ToString();
                var response = await computersDbContext.Computers.AddAsync(entity);
                await computersDbContext.SaveChangesAsync();
                return response.Entity;
            }
        }


        public async Task<IEnumerable<Computer>> GetAsync()
        {
            using (var computersDbContext =
                new ComputersDbContext(
                    _configuration["CosmosDbSettings:EndPoint"].ToString(),
                    _configuration["CosmosDbSettings:AccountKey"].ToString(),
                    _configuration["CosmosDbSettings:DatabaseName"].ToString()))
            {

                var profiles = await computersDbContext.Computers.ToListAsync();
                return profiles;
            }
        }

        public async Task<Computer> GetAsync(string id)
        {
            using (var computersDbContext =
                new ComputersDbContext(
                    _configuration["CosmosDbSettings:EndPoint"].ToString(),
                    _configuration["CosmosDbSettings:AccountKey"].ToString(),
                    _configuration["CosmosDbSettings:DatabaseName"].ToString()))
            {

                var profile = await computersDbContext.Computers.FindAsync(id);
                return profile;
            }

        }
    }
}
