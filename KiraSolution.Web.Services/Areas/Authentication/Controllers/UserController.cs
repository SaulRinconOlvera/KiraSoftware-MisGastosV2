using KiraSolution.Web.Services.Areas.Authentication.Resources;
using KiraSolution.Web.Services.Controllers.Base;
using KiraSolution.Web.Services.Other.Configuration;
using KiraSolution.Web.Services.Other.Transaction.Exceptions;
using KiraSolution.Web.Services.Other.Transaction.Executor;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraSudios.Application.IdentityViewModel.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace KiraSolution.Web.Services.Areas.Authentication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseAuthenticatedController<UserViewModel>, IBaseController<UserViewModel>
    {
        //private IRoleControlAppService _roleControlService;
        //private ITrackingTokenAppService _trackingTokenDomainService;
        //private readonly IRefreshTokenAppService _refreshTokenDomainService;

        public UserController(
            IUserAppService service) : base(service)
        {
            //_roleControlService = roleControlService;
            //_refreshTokenDomainService = refreshTokenDomainService;
            //_trackingTokenDomainService = trackingTokenDomainService;
        }


        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login")]
        //public async Task<IActionResult> Login(
        //    [FromBody] UserViewModel viewModel, string origin)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    Func<Task<UserViewModel>> predicate = async () => { 
        //        if(origin is null)  return await ProcessLocalUser(viewModel); 
        //        else return await ProcessSocialNetworkUser(viewModel);
        //    };

        //    var result = await SafeExecutor<UserViewModel>.ExecAsync(predicate);
        //    return ProcessResponse(result);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public override Task<IActionResult> Post([FromBody] UserViewModel viewModel)
        //{
        //    return base.Post(viewModel);
        //}

        [HttpPatch("{Id}")]
        public override async Task<IActionResult> Patch(int Id, [FromBody] JsonPatchDocument<UserViewModel> pathDocument)
        {
            Func<Task<UserViewModel>> predicate = async () => 
                { return await PathDataAsync(Id, pathDocument); };

            var result = await SafeExecutor<UserViewModel>.ExecAsync(predicate);
            return ProcessResponse(result);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Logout")]
        //public async Task<IActionResult> LogOut([FromBody] TokenInformationViewModel dataRefreshToken)
        //{
        //    var tokenError = new { Error = AuthenticationResource.InvalidToken };

        //    if (!ValidateDataRefreshToken(dataRefreshToken)) return BadRequest(tokenError);

        //    string tokenId = Token.GetTokenId(dataRefreshToken.CurrentJWToken);
        //    if (string.IsNullOrWhiteSpace(tokenId)) return BadRequest(tokenError);

        //    var trackingToken = await _trackingTokenDomainService.GetTokenAsync(new Guid(tokenId));
        //    if (trackingToken is null) return BadRequest(tokenError);

        //    if (!Token.ValidateToken(dataRefreshToken.CurrentJWToken, trackingToken, this.GetUser())) // "saulrincon@hotmail.com"
        //        return BadRequest(tokenError);

        //    var tokenRefresh = await _refreshTokenDomainService.GetTokenAsync(dataRefreshToken.RefreshToken);
        //    if (tokenRefresh is null) return BadRequest(tokenError);

        //    if (tokenRefresh.UserId != trackingToken.UserId ||
        //        tokenRefresh.UserId != dataRefreshToken.UserId)
        //        return BadRequest(tokenError);

        //    _trackingTokenDomainService.Remove(trackingToken);

        //    await (_service as IUserAppService).Logout();
        //    return Ok();
        //}

        private async Task<UserViewModel> PathDataAsync(int id, JsonPatchDocument<UserViewModel> pathDocument)
        {
            if (pathDocument is null) throw new Exception(AuthenticationResource.BadPathDocument);
            var userViewModel = await ((IUserAppService)_service).GetForModifyAsync(id);
            if (userViewModel is null) throw new Exception(AuthenticationResource.UserWrong);

            return await ModifyUserAsync(pathDocument, userViewModel);
        }

        private async Task<UserViewModel> ModifyUserAsync(JsonPatchDocument<UserViewModel> pathDocument, UserViewModel userViewModel)
        {
            DeleteOldImageFile(pathDocument, userViewModel);
            pathDocument.ApplyTo(userViewModel, ModelState);

            if (!TryValidateModel(userViewModel)) throw new ModelStateException(ModelState);
            await _service.ModifyAsync(userViewModel);

            userViewModel.PasswordHash = userViewModel.SecurityStamp = null;

            return userViewModel;
        }

        //private async Task<UserViewModel> ProcessLocalUser(UserViewModel viewModel)
        //{
        //    var result = await ((IUserAppService)_service).LoginAsync(viewModel);
        //    return await ProcessResult(result);
        //}

        //private async Task<UserViewModel> ProcessResult(UserViewModel result)
        //{
        //    if (result is null) throw new Exception(AuthenticationResource.UserWrong);

        //    AppConfiguration.RoleControls = await _roleControlService.GetAllAsync(new string[] { "Role", "Control" });
        //    var trackingToken = Token.Build(result);

        //    await ProcessTrackingToken(result.UserName, trackingToken);

        //    result.Token = trackingToken.Token;
        //    return result;
        //}

        //private async Task<UserViewModel> ProcessSocialNetworkUser(UserViewModel viewModel)
        //{
        //    if (viewModel.Logins is null || !viewModel.Logins.Any())
        //        throw new Exception(AuthenticationResource.NoSocialNetworkUser);

        //    var result = await ((IUserAppService)_service).SocialNetwiorkLoginAsync(
        //            viewModel.Logins.FirstOrDefault().ProviderKey,
        //            viewModel.Logins.FirstOrDefault().LoginProvider);

        //    return await ProcessResult(result);

        //    //if (action == "only_login") throw new Exception(AuthenticationResource.UserNotRegistred);
        //    //return await base.Post(viewModel);
        //}

        //private async Task ProcessTrackingToken(string userName, TrackingTokenViewModel trackingToken)
        //{
        //    await _trackingTokenDomainService.DisableOldTokens(userName);
        //    await _trackingTokenDomainService.AddAsync(trackingToken);

        //    ProcessIdentity(trackingToken);
        //}

        //private void ProcessIdentity(TrackingTokenViewModel trackingToken)
        //{
        //    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
        //    identity.AddClaims(trackingToken.Claims);
        //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        //}



        //[HttpPost]
        //[AllowAnonymous]
        //[Route("RefreshToken")]
        //public async Task<IActionResult> RefreshToken(
        //    [FromBody] TokenInformationViewModel dataRefreshToken)
        //{
        //    var tokenError = new { Error = AuthenticationResource.InvalidToken };

        //    if (dataRefreshToken is null ||
        //        string.IsNullOrWhiteSpace(dataRefreshToken.CurrentJWToken) ||
        //        string.IsNullOrWhiteSpace(dataRefreshToken.RefreshToken))
        //        return BadRequest(tokenError);

        //    string tokenId = Token.GetTokenId(dataRefreshToken.CurrentJWToken);
        //    if (string.IsNullOrWhiteSpace(tokenId)) return BadRequest(tokenError);

        //    var trackingToken =
        //        await _trackingTokenDomainService.GetTokenAsync(new Guid(tokenId));

        //    if (trackingToken is null) return BadRequest(tokenError);

        //    if (!Token.ValidateToken(dataRefreshToken.CurrentJWToken, trackingToken, this.GetUser())) // "saulrincon@hotmail.com"
        //        return BadRequest(tokenError);

        //    var tokenRefresh = await _refreshTokenDomainService.GetTokenAsync(dataRefreshToken.RefreshToken);
        //    if (tokenRefresh is null) return BadRequest(tokenError);

        //    if (tokenRefresh.UserId != trackingToken.UserId)
        //        return BadRequest(tokenError);

        //    var userResult = await _service.GetAsync(tokenRefresh.UserId);
        //    if (userResult is null) return BadRequest(tokenError);

        //    return await ProcessResult(userResult);
        //}






        

        //[NonAction]
        //private bool ValidateDataRefreshToken(TokenInformationViewModel dataRefreshToken)
        //{
        //    return dataRefreshToken != null &&
        //        !string.IsNullOrWhiteSpace(dataRefreshToken.CurrentJWToken) &&
        //        !string.IsNullOrWhiteSpace(dataRefreshToken.RefreshToken);
        //}

        //private async Task<IActionResult> ProcessLocalUser(UserViewModel viewModel)
        //{
        //    var result = await ((IUserAppService)_service).LoginAsync(viewModel);
        //    return await ProcessResult(result);
        //}

        //private async Task<IActionResult> ProcessResult(UserViewModel result)
        //{
        //    if (result != null)
        //    {
        //        await GetRoleControlsAsync();

        //        var trackingToken = Token.Build(result);
        //        await _trackingTokenDomainService.DisableOldTokens(result.UserName);
        //        await _trackingTokenDomainService.AddAsync(trackingToken);

        //        var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
        //        identity.AddClaims(trackingToken.Claims);
        //        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //        result.Token = trackingToken.Token;
        //        return Ok(result);
        //    }
        //    else return BadRequest(AuthenticationResource.UserWrong);
        //}

        //private async Task GetRoleControlsAsync()
        //{
        //    var result = await _roleControlService.GetAllAsync(new string[] { "Role", "Control" });
        //    AppConfiguration.RoleControls = result;
        //}

        //private async Task<IActionResult> ProcessSocialNetworkUser(UserViewModel viewModel, string origin, string action)
        //{
        //    if (viewModel.Logins is null || !viewModel.Logins.Any())
        //        return BadRequest(AuthenticationResource.NoSocialNetworkUser);

        //    var result = await ((IUserAppService)_service).SocialNetwiorkLoginAsync(
        //            viewModel.Logins.FirstOrDefault().ProviderKey,
        //            viewModel.Logins.FirstOrDefault().LoginProvider);

        //    if (result != null) return await ProcessResult(result);
        //    if (action == "only_login") return NotFound(AuthenticationResource.UserNotRegistred);
        //    return await base.Post(viewModel);
        //}

        private void DeleteOldImageFile(JsonPatchDocument<UserViewModel> pathDocument, UserViewModel userViewModel)
        {
            var avatar = pathDocument.Operations.Where(o => o.path == "/avatar").FirstOrDefault();
            if (avatar is null || userViewModel.Avatar is null) return;

            string file = Path.Combine(Directory.GetCurrentDirectory(), AppConfiguration.ImagesPath, userViewModel.Avatar);
            if (System.IO.File.Exists(file))
            {
                try { System.IO.File.Delete(file); }
                catch { }
            }
        }
    }
}
