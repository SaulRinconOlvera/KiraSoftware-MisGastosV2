using KiraStudios.Application.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiraStudios.Application.ServiceBase
{
    public interface IApplicationServiceBase<TKey, TViewModel>
        where TViewModel : class, IBaseViewModel<TKey>
    {
        //  User
        void SetUser(string user);

        //  Get
        TViewModel Get(TKey viewModelId);
        IEnumerable<TViewModel> GetAll();
        IEnumerable<TViewModel> GetAll(params string[] includes);
        IEnumerable<TViewModel> GetAllMatching(
            Expression<Func<TViewModel, bool>> filter,
            params string[] includes);
        IEnumerable<TViewModel> GetAllMatchingPaged(
            Expression<Func<TViewModel, bool>> filter,
            int pageNumber, int pageSize, params string[] includes);

        //  Get Async
        Task<TViewModel> GetAsync(TKey viewModelId);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<IEnumerable<TViewModel>> GetAllAsync(params string[] includes);
        Task<IEnumerable<TViewModel>> GetAllMatchingAsync(
            Expression<Func<TViewModel, bool>> filter,
            params string[] includes);
        Task<IEnumerable<TViewModel>> GetAllMatchingPagedAsync(
            Expression<Func<TViewModel, bool>> filter,
            int pageNumber, int pageSize, params string[] includes);

        //  Add
        void Add(TViewModel viewModel);
        TViewModel AddWithResponse(TViewModel viewModel);
        void AddRange(IEnumerable<TViewModel> viewModels);
        IEnumerable<TViewModel> AddRangeWithResponse(IEnumerable<TViewModel> viewModels);

        //  AddAsync
        Task AddAsync(TViewModel viewModel);
        Task<TViewModel> AddWithResponseAsync(TViewModel viewModel);
        Task AddRangeAsync(IEnumerable<TViewModel> entities);
        Task<IEnumerable<TViewModel>> AddRangeWithResponseAsync(IEnumerable<TViewModel> entities);

        //  Modify
        void Modify(TViewModel viewModel);
        Task ModifyAsync(TViewModel viewModel);
        TViewModel ModifyWithResponse(TViewModel viewModel);
        Task<TViewModel> ModifyWithResponseAsync(TViewModel viewModel);

        //  Remove
        bool Remove(TViewModel viewModel, bool applyPhysical = false);
        bool RemoveRange(IEnumerable<TViewModel> viewModels, bool applyPhysical = false);

        bool Remove(TKey viewModelId, bool applyPhysical = false);
        bool RemoveRange(IEnumerable<TKey> viewModelIds, bool applyPhysical = false);

        //  Save
        int Save();
        Task<int> SaveAsync();
    }
}
