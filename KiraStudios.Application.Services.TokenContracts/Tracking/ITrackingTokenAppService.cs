using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.TokenViewModel.Tracking;
using System;
using System.Threading.Tasks;

namespace KiraStudios.Application.Services.TokenContracts.Tracking
{
    public interface ITrackingTokenAppService : IApplicationServiceBase<int, TrackingTokenViewModel>
    {
        Task DisableOldTokens(string UserName);
        Task<TrackingTokenViewModel> GetTokenAsync(Guid tokenId);
    }
}
