using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.TokenModel.Tracking;
using KiraStudios.Domain.TokenRepository;
using KiraStudios.Infrastructure.RepositoryBase;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KiraStudios.Infraestructure.TrackingRepository.Tracking
{
    public class TrackingTokenRepository : RepositoryBase<int, TrackingToken>, ITrackingTokenRepository
    {
        public TrackingTokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<TrackingToken> GetTokenAsync(Guid tokenId)
        {
            var res = await GetAllMatchingAsync(t => t.TokenId == tokenId);
            return res.FirstOrDefault();
        }
    }
}
