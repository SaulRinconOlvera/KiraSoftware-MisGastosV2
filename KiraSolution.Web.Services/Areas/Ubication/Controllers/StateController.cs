using KiraSolution.Web.Services.Controllers.Base;
using KiraStudios.Application.Services.UbicationContracts.Ubication;
using KiraStudios.Application.UbicationViewModel.Ubication;

namespace KiraSolution.Web.Services.Areas.Ubication.Controllers
{
    public class StateController : BaseAuthenticatedController<StateViewModel>,
        IBaseController<StateViewModel>
    {
        public StateController(IStateAppService service)
            : base(service) { }
    }
}
