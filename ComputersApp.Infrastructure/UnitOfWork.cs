using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ComputersContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ComputersContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            lock (_context)
            {
                return _context.SaveChangesAsync().Result;
            }
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : IEntityBase
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
