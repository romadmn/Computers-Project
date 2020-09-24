using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComputersApp.Domain
{
    public interface IRepository<TEntity> where TEntity : IEntityBase
    {
        Task<TEntity> FindById(int id);
        IEnumerable<TEntity> Find(ISpecification<TEntity> specification = null);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        bool Contains(ISpecification<TEntity> specification = null);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        int Count(ISpecification<TEntity> specification = null);
        int Count(Expression<Func<TEntity, bool>> predicate);
    }
}
