using KiraSolution.Web.Services.Controllers.Base;
using KiraStudios.Application.Services.UbicationContracts.Ubication;
using KiraStudios.Application.UbicationViewModel.Ubication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiraSolution.Web.Services.Areas.Ubication.Controllers
{
    public class CountryController : BaseAuthenticatedController<CountryViewModel>, 
        IBaseController<CountryViewModel>
    {
        public CountryController(ICountryAppService service)
            : base(service) { }

        [HttpGet("WithStates")]
        public async Task<IEnumerable<CountryViewModel>> GetWhitStatesAsync() =>
           await _service.GetAllAsync(new string[] { "States" });
    }
}
