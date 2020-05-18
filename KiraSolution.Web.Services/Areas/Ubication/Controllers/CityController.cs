using KiraSolution.Web.Services.Controllers.Base;
using KiraStudios.Application.Services.UbicationContracts.Ubication;
using KiraStudios.Application.UbicationViewModel.Ubication;

namespace KiraSolution.Web.Services.Areas.Ubication.Controllers
{
    public class CityController : BaseAuthenticatedController<CityViewModel>,
        IBaseController<CityViewModel>
    {
        public CityController(ICityAppService service)
            : base(service) { }
    }
}
