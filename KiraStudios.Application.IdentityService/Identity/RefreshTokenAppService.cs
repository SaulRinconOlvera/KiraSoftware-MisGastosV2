using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraSudios.Application.IdentityViewModel.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace KiraStudios.Application.IdentityService.Identity
{
    public class RefreshTokenAppService :
        ApplicationServiceBase<RefreshToken, RefreshTokenViewModel>, IRefreshTokenAppService
    {
        public RefreshTokenAppService(
            IRefreshTokenRepository repository) { _repository = repository; }

        public async Task<RefreshTokenViewModel> GetTokenAsync(string tokenId)
        {
            var res = await _repository.GetAllMatchingAsync(tr => tr.Token == tokenId);
            var tokenRefresh = res.FirstOrDefault();

            if (tokenRefresh is null) return null;
            return _mapper.GetViewModel(tokenRefresh);
        }
    }
}
