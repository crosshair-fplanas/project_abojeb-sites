using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AbojebApi.Data.Interfaces.Members
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void Modify(TEntity entity);

        IQueryable<TEntity> Collections { get; }
    }
}
