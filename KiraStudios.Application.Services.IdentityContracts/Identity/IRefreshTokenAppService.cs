using KiraStudios.Application.ServiceBase;
using KiraSudios.Application.IdentityViewModel.Identity;
using System.Threading.Tasks;

namespace KiraStudios.Application.Services.IdentityContracts.Identity
{
    public interface IRefreshTokenAppService : IApplicationServiceBase<int, RefreshTokenViewModel>
    {
        Task<RefreshTokenViewModel> GetTokenAsync(string tokenId);
    }
}
