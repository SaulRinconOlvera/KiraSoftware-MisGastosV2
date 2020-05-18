using System.Collections.Generic;

namespace KiraStudios.Application.MapperBase
{
    public interface IGenericMapper<T, TViewModel>
       where T : class where TViewModel : class
    {
        T GetEntity(TViewModel viewModel);
        IEnumerable<T> GetEntities(IEnumerable<TViewModel> viewModels);
        TViewModel GetViewModel(T entity);
        IEnumerable<TViewModel> GetViewModels(IEnumerable<T> entities);
    }
}
