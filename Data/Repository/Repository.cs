using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private AppDbContext _context;

        //protected AppDbContext DbContext
        //{
        //    get { return _context ?? (_context = new AppDbContext()); }
        //}

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public virtual void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public virtual IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _context.Set<T>().Where(where);
        }

        public virtual T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            var entity = _context.Set<T>().Where(where).FirstOrDefault();
            return entity;
        }

        public virtual void Remove(object id)
        {
            var entity = GetById(id);
            Remove(entity);
        }

        public virtual void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> Pagination(Expression<Func<T, bool>> where)
        {
            //_context.Set<T>().;
            return _context.Set<T>();
        }
    }
}
