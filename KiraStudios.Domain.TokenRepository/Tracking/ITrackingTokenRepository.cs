using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.TokenModel.Tracking;
using System;
using System.Threading.Tasks;

namespace KiraStudios.Domain.TokenRepository
{
    public interface ITrackingTokenRepository : IRepositoryBase<int, TrackingToken>
    {
        Task<TrackingToken> GetTokenAsync(Guid tokenId);
    }
}
