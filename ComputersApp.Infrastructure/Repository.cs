using ComputersApp.Domain;
using ComputersApp.Domain.Entities;
using ComputersApp.Infrastructure.Specifications;
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

        public virtual IEnumerable<TEntity> Find(ISpecification<TEntity> specification = null)
        {
            return ApplySpecification(specification);
        }

        public virtual async Task<TEntity> FindById(int id)
        {
            return await Entity.FindAsync(id);
        }

        public virtual async Task Add(TEntity entity)
        {
            await Entity.AddAsync(entity);
        }
        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await Entity.AddRangeAsync(entities);
        }

        public virtual bool Contains(ISpecification<TEntity> specification = null)
        {
            return Count(specification) > 0 ? true : false;
        }

        public virtual bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return Count(predicate) > 0 ? true : false;
        }

        public virtual int Count(ISpecification<TEntity> specification = null)
        {
            return ApplySpecification(specification).Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Entity.Where(predicate).Count();
        }

        public virtual void Remove(TEntity entity)
        {
            Entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entity.RemoveRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Entity.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(Entity.AsQueryable(), spec);
        }

    }
}
