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
        IQueryable<TEntity> GetAll();
        Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
