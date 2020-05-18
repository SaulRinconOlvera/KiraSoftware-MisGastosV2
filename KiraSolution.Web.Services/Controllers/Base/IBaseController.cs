using KiraStudios.Application.ViewModelBase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KiraSolution.Web.Services.Controllers.Base
{
    public interface IBaseController<IViewModel>
        where IViewModel : class, IBaseViewModel<int> 
    {
        Task<IActionResult> Get();
        Task<IActionResult> Get(int id);

        Task<IActionResult> Post([FromBody] IViewModel viewModel);
        Task<IActionResult> Put(int id, [FromBody] IViewModel viewModel);
        Task<IActionResult> Delete(int id);
    }
}
