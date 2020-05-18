using KiraStudios.Application.MapperBase;
using KiraStudios.Application.ViewModelBase;
using KiraStudios.Domain.EntityBase.Contracts;
using KiraStudios.Domain.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiraStudios.Application.ServiceBase
{
    public abstract class ApplicationServiceBase<TEntity, TViewModel> :
        IApplicationServiceBase<int, TViewModel>
        where TEntity : class, IBaseEntity<int>, IBaseAuditable
        where TViewModel : class, IBaseViewModel<int>
    {
        protected IGenericMapper<TEntity, TViewModel> _mapper;
        protected string _serviceUser;
        protected IRepositoryBase<int, TEntity> _repository;
        private bool _autoSave = true;

        public void SetAutoSave(bool autoSave) => _autoSave = autoSave;

        public ApplicationServiceBase()
        {
            _mapper = new GenericMapper<TEntity, TViewModel>();
            _serviceUser = "SYSTEM";
        }

        public virtual void Add(TViewModel viewModel)
        {
            var entity = SetUserOnAdd(_mapper.GetEntity(viewModel), _serviceUser);
            _repository.Add(entity, _autoSave);
        }

        public virtual TViewModel AddWithResponse(TViewModel viewModel)
        {
            var entity = SetUserOnAdd(_mapper.GetEntity(viewModel), _serviceUser);
            _repository.Add(entity, _autoSave);
            return _mapper.GetViewModel(entity);
        }

        public virtual async Task AddAsync(TViewModel viewModel)
        {
            var entity = _mapper.GetEntity(viewModel);
            entity = SetUserOnAdd(entity, _serviceUser);
            await _repository.AddAsync(entity, _autoSave);
        }

        public virtual async Task<TViewModel> AddWithResponseAsync(TViewModel viewModel)
        {
            var entity = SetUserOnAdd(_mapper.GetEntity(viewModel), _serviceUser);
            await _repository.AddAsync(entity, _autoSave);
            return _mapper.GetViewModel(entity);
        }

        public virtual void AddRange(IEnumerable<TViewModel> viewModels)
        {
            var entities = _mapper.GetEntities(viewModels);
            entities = SetUserOnAddRange(entities, _serviceUser);
            _repository.AddRange(entities, _autoSave);
        }

        public virtual IEnumerable<TViewModel> AddRangeWithResponse(IEnumerable<TViewModel> viewModels)
        {
            var entities = SetUserOnAddRange(_mapper.GetEntities(viewModels), _serviceUser);
            _repository.AddRange(entities, _autoSave);
            return _mapper.GetViewModels(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TViewModel> viewModels)
        {
            var entities = _mapper.GetEntities(viewModels);
            entities = SetUserOnAddRange(entities, _serviceUser);
            await _repository.AddRangeAsync(entities, _autoSave);
        }

        public virtual async Task<IEnumerable<TViewModel>>
            AddRangeWithResponseAsync(IEnumerable<TViewModel> viewModels)
        {
            var entities = SetUserOnAddRange(_mapper.GetEntities(viewModels), _serviceUser);
            await _repository.AddRangeAsync(entities, _autoSave);
            return _mapper.GetViewModels(entities);
        }

        public virtual void Modify(TViewModel viewModel)
        {
            var entity = SetAudatableData(viewModel);
            _repository.Modify(entity, _autoSave);
        }

        public virtual async Task ModifyAsync(TViewModel viewModel)
        {
            var entity = SetAudatableData(viewModel);
            await _repository.ModifyAsync(entity, _autoSave);
        }

        public virtual TViewModel ModifyWithResponse(TViewModel viewModel)
        {
            var entity = SetAudatableData(viewModel);
            _repository.Modify(entity, _autoSave);
            return _mapper.GetViewModel(entity);
        }

        public virtual async Task<TViewModel> ModifyWithResponseAsync(TViewModel viewModel)
        {
            var entity = SetAudatableData(viewModel);
            await _repository.ModifyAsync(entity, _autoSave);
            return _mapper.GetViewModel(entity);
        }

        public virtual TViewModel Get(int viewModelId) =>
           _mapper.GetViewModel(_repository.Get(viewModelId));

        public virtual async Task<TViewModel> GetAsync(int viewModelId) =>
           _mapper.GetViewModel(await _repository.GetAsync(viewModelId));

        public virtual IEnumerable<TViewModel> GetAll() =>
            _mapper.GetViewModels(_repository.GetAll());

        public virtual IEnumerable<TViewModel> GetAll(params string[] includes) =>
            _mapper.GetViewModels(_repository.GetAll(includes));

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync() =>
            _mapper.GetViewModels(await _repository.GetAllAsync());

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync(params string[] includes) =>
            _mapper.GetViewModels(await _repository.GetAllAsync(includes));

        public virtual IEnumerable<TViewModel> GetAllMatching(
            Expression<Func<TViewModel, bool>> filter, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<TViewModel>> GetAllMatchingAsync(
            Expression<Func<TViewModel, bool>> filter, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TViewModel> GetAllMatchingPaged(
            Expression<Func<TViewModel, bool>> filter, int pageNumber, int pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<TViewModel>> GetAllMatchingPagedAsync(
            Expression<Func<TViewModel, bool>> filter, int pageNumber, int pageSize, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(TViewModel viewModel, bool applyPhysical = false)
        {
            var res = SetUserOnDelete(_mapper.GetEntity(viewModel), _serviceUser);
            return _repository.Remove(res, applyPhysical, _autoSave); 
        }

        [Obsolete("This method is deprecated, please use Remove(object) instead.")]
        public virtual bool Remove(int viewModelId, bool applyPhysical = false) =>
            _repository.Remove(viewModelId, applyPhysical, _autoSave);

        public virtual bool RemoveRange(IEnumerable<TViewModel> viewModels, bool applyPhysical = false)
        {
            var res = SetUserOnDelete(_mapper.GetEntities(viewModels), _serviceUser);
            return _repository.RemoveRange(res, applyPhysical, _autoSave);
        }

        [Obsolete("This method is deprecated, please use RemoveRange(IEnumerable<object>) instead.")]
        public virtual bool RemoveRange(IEnumerable<int> viewModelIds, bool applyPhysical = false) =>
            _repository.RemoveRange(viewModelIds, applyPhysical, _autoSave);

        public virtual int Save() => _repository.Save();

        public virtual Task<int> SaveAsync() => _repository.SaveAsync();

        //  Usuarios
        private TEntity SetUserOnAdd(TEntity entity, string user)
        {
            entity.CreatedBy = entity.LastModifiedBy = user; entity.Enabled = true;
            entity.CreationDate = entity.LastModificationDate = DateTime.Now;
            return entity;
        }

        private static IEnumerable<TEntity> SetUserOnAddRange(
            IEnumerable<TEntity> entities, string user)
        {
            entities.ToList().ForEach(e =>
            {
                e.Enabled = true;
                e.CreatedBy = e.LastModifiedBy = user;
                e.CreationDate = e.LastModificationDate = DateTime.Now;

            });

            return entities;
        }

        private static TEntity SetUserOnModify(TEntity entity, string user)
        {
            entity.LastModifiedBy = user;
            entity.LastModificationDate = DateTime.Now;
            return entity;
        }

        private static IEnumerable<TEntity> SetUserOnDelete(IEnumerable<TEntity> entities, string user)
        {
            entities.ToList().ForEach(e => SetUserOnDelete(e, user));
            return entities;
        }

        private static TEntity SetUserOnDelete(TEntity entity, string user)
        {
            entity.DeletedBy = user;
            entity.DeletionDate = DateTime.Now;
            entity.Enabled = false;
            return entity;
        }

        private TEntity SetAudatableData(TViewModel viewModel)
        {
            var entity = _mapper.GetEntity(viewModel);
            return SetUserOnModify(entity, _serviceUser);
        }

        public void SetUser(string user) =>
            _serviceUser = user.Equals(string.Empty) ? "SYSTEM" : user;
    }
}
