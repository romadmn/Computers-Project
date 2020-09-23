using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly ComputersContext Context;
        protected DbSet<TEntity> Entity;

        public Repository(ComputersContext context)
        {
            Context = context;
            Entity = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Entity.AsQueryable();
        }

        public virtual async Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entity.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task Add(TEntity entity)
        {
            await Entity.AddAsync(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Entity.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Entity.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            lock (Context)
            {
                return Context.SaveChangesAsync().Result;
            }
        }
    }
}
