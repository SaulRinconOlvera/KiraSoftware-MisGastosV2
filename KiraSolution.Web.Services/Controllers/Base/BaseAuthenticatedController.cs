using KiraSolution.Web.Services.Other.Configuration;
using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.ViewModelBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace KiraSolution.Web.Services.Controllers.Base
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = AppConfiguration.GeneralAccessPolicyName)]
    public class BaseAuthenticatedController<TViewModel> : BaseController<TViewModel>
        where TViewModel : class, IBaseViewModel<int>
    {
        public BaseAuthenticatedController(IApplicationServiceBase<int, TViewModel> service) : base(service)
        { }
    }
}
