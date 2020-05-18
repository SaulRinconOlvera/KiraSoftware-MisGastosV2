using KiraStudios.Domain.EntityBase.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiraStudios.Domain.RepositoryBase
{
    public interface IRepositoryBase<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>
    {
        //  Get
        TEntity Get(TKey entityId);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(params string[] includes);
        IEnumerable<TEntity> GetAllMatching(
            Expression<Func<TEntity, bool>> filter,
            params string[] includes);
        IEnumerable<TEntity> GetAllMatchingPaged(
            Expression<Func<TEntity, bool>> filter,
            int pageNumber, int pageSize, params string[] includes);

        //  Get Async
        Task<TEntity> GetAsync(TKey entityId);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(params string[] includes);
        Task<IEnumerable<TEntity>> GetAllMatchingAsync(
            Expression<Func<TEntity, bool>> filter,
            params string[] includes);
        Task<IEnumerable<TEntity>> GetAllMatchingPagedAsync(
            Expression<Func<TEntity, bool>> filter,
            int pageNumber, int pageSize, params string[] includes);

        //  Add
        void Add(TEntity entity, bool autoSave = true);
        void AddRange(IEnumerable<TEntity> entities, bool autoSave = true);

        //  AddAsync
        Task AddAsync(TEntity entity, bool autoSave = true);
        Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = true);

        //  Modify
        void Modify(TEntity entity, bool autoSave = true);
        Task ModifyAsync(TEntity entity, bool autoSave = true);

        //  Remove
        bool Remove(TEntity entity, bool applyPhysical = false, bool autoSave = true);
        bool RemoveRange(IEnumerable<TEntity> entities, bool applyPhysical = false, bool autoSave = true);

        bool Remove(TKey entityId, bool applyPhysical = false, bool autoSave = true);
        bool RemoveRange(IEnumerable<TKey> entitiesIds, bool applyPhysical = false, bool autoSave = true);

        //  Save
        int Save();

        //  SQL
        Task ExecSqlCommandAsync(string command);
        Task<int> SaveAsync();
    }
}
