using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetMany(Expression<Func<T, bool>> where);

        void Insert(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(object id);

        IQueryable<T> GetAll();

        T Get(Expression<Func<T, bool>> where);

        T GetById(object id);

        void RemoveMultiple(List<T> entities);
    }
}
