using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.Services.TokenContracts.Tracking;
using KiraStudios.Application.TokenViewModel.Tracking;
using KiraStudios.Domain.TokenModel.Tracking;
using KiraStudios.Domain.TokenRepository;
using System;
using System.Threading.Tasks;

namespace KiraStudios.Application.TokenService.Tracking
{
    public class TrackingTokenAppService :
        ApplicationServiceBase<TrackingToken, TrackingTokenViewModel>, ITrackingTokenAppService
    {
        public TrackingTokenAppService(ITrackingTokenRepository repository) { _repository = repository; }

        public async Task DisableOldTokens(string UserName) =>
            await _repository.ExecSqlCommandAsync($"Exec DisableOldTokens '{UserName}'");

        public async Task<TrackingTokenViewModel> GetTokenAsync(Guid tokenId)
        {
            var token = await (_repository as ITrackingTokenRepository).GetTokenAsync(tokenId);
            if (token is null) return null;

            return _mapper.GetViewModel(token);
        }
    }
}
