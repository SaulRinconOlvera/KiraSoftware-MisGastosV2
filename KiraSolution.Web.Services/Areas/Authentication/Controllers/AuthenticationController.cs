using KiraSolution.Web.Services.Areas.Authentication.Resources;
using KiraSolution.Web.Services.Controllers.Base;
using KiraSolution.Web.Services.Other.Configuration;
using KiraSolution.Web.Services.Other.JWT;
using KiraSolution.Web.Services.Other.Transaction.Executor;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraStudios.Application.Services.TokenContracts.Tracking;
using KiraStudios.Application.TokenViewModel.Tracking;
using KiraSudios.Application.IdentityViewModel.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KiraSolution.Web.Services.Areas.Authentication.Controllers
{
    public class AuthenticationController : BaseController<UserViewModel>,
        IBaseController<UserViewModel>
    {
        private IRoleControlAppService _roleControlService;
        private ITrackingTokenAppService _trackingTokenDomainService;
        private readonly IRefreshTokenAppService _refreshTokenDomainService;

        public AuthenticationController(
            IUserAppService service,
            ITrackingTokenAppService trackingTokenDomainService,
            IRefreshTokenAppService refreshTokenDomainService,
            IRoleControlAppService roleControlService) : base(service)
        {
            _roleControlService = roleControlService;
            _refreshTokenDomainService = refreshTokenDomainService;
            _trackingTokenDomainService = trackingTokenDomainService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(
            [FromBody] UserViewModel viewModel, string origin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Func<Task<UserViewModel>> predicate = async () => {
                if (origin is null) return await ProcessLocalUser(viewModel);
                else return await ProcessSocialNetworkUser(viewModel);
            };

            var result = await SafeExecutor<UserViewModel>.ExecAsync(predicate);
            return ProcessResponse(result);
        }

        private async Task<UserViewModel> ProcessLocalUser(UserViewModel viewModel)
        {
            var result = await ((IUserAppService)_service).LoginAsync(viewModel);
            return await ProcessResult(result);
        }

        private async Task<UserViewModel> ProcessResult(UserViewModel result)
        {
            if (result is null) throw new Exception(AuthenticationResource.UserWrong);

            AppConfiguration.RoleControls = await _roleControlService.GetAllAsync(new string[] { "Role", "Control" });
            var trackingToken = Token.Build(result);

            await ProcessTrackingToken(result.UserName, trackingToken);

            result.Token = trackingToken.Token;
            return result;
        }

        private async Task ProcessTrackingToken(string userName, TrackingTokenViewModel trackingToken)
        {
            await _trackingTokenDomainService.DisableOldTokens(userName);
            await _trackingTokenDomainService.AddAsync(trackingToken);

            ProcessIdentity(trackingToken);
        }

        private void ProcessIdentity(TrackingTokenViewModel trackingToken)
        {
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(trackingToken.Claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        }

        private async Task<UserViewModel> ProcessSocialNetworkUser(UserViewModel viewModel)
        {
            if (viewModel.Logins is null || !viewModel.Logins.Any())
                throw new Exception(AuthenticationResource.NoSocialNetworkUser);

            var result = await ((IUserAppService)_service).SocialNetwiorkLoginAsync(
                    viewModel.Logins.FirstOrDefault().ProviderKey,
                    viewModel.Logins.FirstOrDefault().LoginProvider);

            return await ProcessResult(result);

            //if (action == "only_login") throw new Exception(AuthenticationResource.UserNotRegistred);
            //return await base.Post(viewModel);
        }
    }
}