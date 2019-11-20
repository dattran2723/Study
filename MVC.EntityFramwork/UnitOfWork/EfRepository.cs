using LinqKit;
using MVC.Common.Exceptions;
using MVC.Common.Extensions;
using MVC.Entities.Interfaces;
using MVC.EntityFramwork.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC.EntityFramwork.UnitOfWork
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public DbContext Context;

        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        public EfRepository(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public IQueryable<TEntity> GetExpandableQuery()
        {
            return GetAll().AsExpandable();
        }

        public IQueryable<TEntity> GetIncludingExpandableQuery(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetAll();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query.AsExpandable();
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().AnyAsync(predicate);
        }

        public TEntity Get(int id)
        {
            var entity = GetAll().FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }


        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public TEntity Insert(TEntity entity)
        {
            return Table.Add(entity);
        }

        //public async Task BulkInsertAsync(TEntity[] entities)
        //{
        //    if (CheckTableAutomaticIdentity())
        //    {
        //        await Table.AddRangeAsync(entities);
        //    }
        //    else
        //    {
        //        int maxId = MaxId();
        //        foreach (var entity in entities)
        //        {
        //            entity.Id = ++maxId;
        //            await Table.AddAsync(entity);
        //        }
        //    }
        //}

        //public void BulkInsert(TEntity[] entities)
        //{
        //    if (CheckTableAutomaticIdentity())
        //    {
        //        Table.AddRange(entities);
        //    }
        //    else
        //    {
        //        int maxId = MaxId();
        //        foreach (var entity in entities)
        //        {
        //            entity.Id = ++maxId;
        //            Table.Add(entity);
        //        }
        //    }
        //}

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public TEntity InsertAndGet(TEntity entity)
        {
            entity = Insert(entity);

            Context.SaveChanges();

            return entity;
        }

        public async Task<TEntity> InsertAndGetAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public TEntity InsertOrUpdateAndGet(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            Context.SaveChanges();

            return entity;
        }

        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return IsTransient(entity)
                ? Insert(entity)
                : Update(entity);
        }

        public async Task<TEntity> InsertOrUpdateAndGetAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return IsTransient(entity)
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        //public void Deactive(TEntity entity)
        //{
        //    if (entity is IActiveableEntity)
        //    {
        //        entity.As<IActiveableEntity>().IsActive = false;
        //    }

        //    Update(entity);
        //}

        //public void Active(TEntity entity)
        //{
        //    if (entity is IActiveableEntity)
        //    {
        //        entity.As<IActiveableEntity>().IsActive = true;
        //    }

        //    Update(entity);
        //}

        //public void Active(int id)
        //{
        //    var entity = Get(id);

        //    if (entity != null)
        //    {
        //        Active(entity);
        //    }
        //}

        //public void Deactive(int id)
        //{
        //    var entity = Get(id);

        //    if (entity != null)
        //    {
        //        Deactive(entity);
        //    }
        //}

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public void Delete(int id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = Get(id);

            if (entity != null)
            {
                Delete(entity);
            }

            //Could not found the entity, do nothing.
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public virtual Task DeleteAsync(int id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
            return Task.FromResult(0);
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(int id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<int>.Default.Equals(id, ((TEntity)ent.Entity).Id)
                );

            return entry?.Entity as TEntity;
        }

        //private void CheckAndSetId(TEntity entity)
        //{
        //    if (entity.Id == 0 && !CheckTableAutomaticIdentity())
        //    {
        //        entity.Id = MaxId() + 1;
        //    }
        //}

        //private static bool CheckTableAutomaticIdentity()
        //{
        //    var dbGeneratedAttr = ReflectionHelper
        //        .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
        //            typeof(TEntity).GetProperty("Id"));

        //    if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private int MaxId()
        //{
        //    return !Table.IgnoreQueryFilters().Any() ? 0 : Table.IgnoreQueryFilters().Max(x => x.Id);
        //}

        private static bool IsTransient(IEntity entity)
        {
            return entity.Id <= 0;
        }
    }
}
