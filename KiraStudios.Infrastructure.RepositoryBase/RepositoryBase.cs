using KiraStudios.Domain.EntityBase.Contracts;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiraStudios.Infrastructure.RepositoryBase
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepositoryBase<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>, IBaseAuditable
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;
        private bool _tracking = false;
        private IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            dbContext = unitOfWork.Context;
            dbSet = dbContext.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }

        internal void SetUnitOfWork(IUnitOfWork unitOfWork) {
            _unitOfWork.Dispose(); _unitOfWork = unitOfWork;
        }

        public virtual IUnitOfWork UnitOfWork => _unitOfWork;

        //  Get
        public virtual TEntity Get(TKey entityId)
            => _tracking ? dbSet.Where(e => e.Id.Equals(entityId)).FirstOrDefault() :
            dbSet.AsNoTracking().Where(e => e.Id.Equals(entityId)).FirstOrDefault();

        public virtual IEnumerable<TEntity> GetAll()
            => _tracking ? dbSet.ToList() : dbSet.AsNoTracking().ToList();

        public virtual IEnumerable<TEntity> GetAll(params string[] includes)
             => _tracking ? DoGetAll(includes).ToList() :
            DoGetAll(includes).AsNoTracking().ToList();

        public virtual IEnumerable<TEntity> GetAllMatching(
            Expression<Func<TEntity, bool>> filter, params string[] includes)
            => _tracking ? DoGetAllMatching(filter, includes).ToList() :
            DoGetAllMatching(filter, includes).AsNoTracking().ToList();

        public virtual IEnumerable<TEntity> GetAllMatchingPaged(
            Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize, params string[] includes)
             => _tracking ? DoGetAllMatching(filter, includes).OrderBy(e => e.Id)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList() :
            DoGetAllMatching(filter, includes).AsNoTracking().OrderBy(e => e.Id)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();

        //  GetAsync
        public virtual async Task<TEntity> GetAsync(TKey entityId)
             => _tracking ?
            await dbSet.Where(e => e.Id.Equals(entityId)).FirstOrDefaultAsync() :
            await dbSet.AsNoTracking().Where(e => e.Id.Equals(entityId)).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => _tracking ? await dbSet.ToListAsync() : await dbSet.AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params string[] includes)
            => _tracking ? await DoGetAll(includes).ToListAsync() :
            await DoGetAll(includes).AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllMatchingAsync(
            Expression<Func<TEntity, bool>> filter, params string[] includes)
            => _tracking ? await DoGetAllMatching(filter, includes).ToListAsync().ConfigureAwait(false) :
            await DoGetAllMatching(filter, includes).AsNoTracking().ToListAsync().ConfigureAwait(false);

        public virtual async Task<IEnumerable<TEntity>> GetAllMatchingPagedAsync(
            Expression<Func<TEntity, bool>> filter, int pageNumber, int pageSize, params string[] includes)
            => _tracking ? await DoGetAllMatching(filter, includes).OrderBy(e => e.Id)
                .Skip(pageNumber * pageSize).Take(pageSize).ToListAsync() :
            await DoGetAllMatching(filter, includes).AsNoTracking().OrderBy(e => e.Id)
                .Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

        //  Add
        public virtual void Add(TEntity entity, bool autoSave = true)
        { dbSet.Add(entity); if (autoSave) Save(); }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool autoSave = true)
        { dbSet.AddRange(entities); if (autoSave) Save(); }

        //  AddAsync
        public virtual async Task AddAsync(TEntity entity, bool autoSave = true)
        { await dbSet.AddAsync(entity); if (autoSave) await SaveAsync(); }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = true)
        { await dbSet.AddRangeAsync(entities); if (autoSave) await SaveAsync(); }

        //  Modify
        public virtual void Modify(TEntity entity, bool autoSave = true)
        { SetModified(entity); if (autoSave) Save(); }

        public virtual async Task ModifyAsync(TEntity entity, bool autoSave = true)
        { SetModified(entity); if (autoSave) await SaveAsync(); }

        //  Remove
        public virtual bool Remove(TEntity entity, bool applyPhysical = false, bool autoSave = true)
        {
            if (!applyPhysical) SetLogicalDelete(entity);
            else { dbSet.Attach(entity); dbSet.Remove(entity); }

            if (autoSave) Save(); return true;
        }

        public virtual bool Remove(TKey entityId, bool applyPhysical = false, bool autoSave = true)
        { Remove(dbSet.Find(entityId), applyPhysical); if (autoSave) Save(); return true; }

        public virtual bool RemoveRange(IEnumerable<TEntity> entities, bool applyPhysical = false, bool autoSave = true)
        {
            if (!applyPhysical) entities.ToList().ForEach(e => SetLogicalDelete(e));
            else { dbSet.AttachRange(entities); dbSet.RemoveRange(entities); }

            if (autoSave) Save(); return true;
        }

        public virtual bool RemoveRange(IEnumerable<TKey> entitiesIds, bool applyPhysical = false, bool autoSave = true)
        { entitiesIds.ToList().ForEach(eid => Remove(eid, applyPhysical)); if (autoSave) Save(); return true; }

        //  Save
        public virtual int Save() => dbContext.SaveChanges();
        public virtual Task<int> SaveAsync() => dbContext.SaveChangesAsync();

        internal IQueryable<TEntity> DoGetAll(params string[] includes)
            => GetQueryable(includes: includes);

        internal IQueryable<TEntity> DoGetAllMatching(
            Expression<Func<TEntity, bool>> filter, params string[] includes)
        {
            return GetQueryable(filter, includes: includes);
        }

        internal IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null, int pageGo = 0, int pageSize = 0,
            List<string> orderBy = null, bool orderAscendent = false, params string[] includes)
        {
            IQueryable<TEntity> items = dbContext.Set<TEntity>();
            items.AsNoTracking();

            if (includes != null && includes.Any())
                includes.Where(i => i != null).ToList().ForEach(i => items = items.Include(i));

            if (filter != null) items = items.Where(filter);

            if (pageSize > 0)
            {
                if (orderBy != null && orderBy.Any())
                {
                    orderBy.Where(i => i != null).ToList().ForEach(
                        s => items = QueryableUtils.CallOrderBy(items, s, orderAscendent));
                }
                items = items.Take(pageSize);
            }

            return items;
        }

        private void SetLogicalDelete(TEntity entity)
        {
            entity.GetType().GetProperty("Enabled").SetValue(entity, false);
            SetModified(entity, isDelete: true);
        }

        internal void SetModified(TEntity entity, bool isDelete = false)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.Entry(entity).Property(p => p.Id).IsModified = false;
            dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            dbContext.Entry(entity).Property(p => p.CreationDate).IsModified = false;
            dbContext.Entry(entity).Property(p => p.LastModificationDate).IsModified = !isDelete;
            dbContext.Entry(entity).Property(p => p.LastModifiedBy).IsModified = !isDelete;
            dbContext.Entry(entity).Property(p => p.DeletedBy).IsModified = isDelete;
            dbContext.Entry(entity).Property(p => p.DeletionDate).IsModified = isDelete;
        }

        public async Task ExecSqlCommandAsync(string command)
        {
            //await dbContext.Database.ExecuteSqlRawAsync(command);
            await SaveAsync();
        }

    }
}