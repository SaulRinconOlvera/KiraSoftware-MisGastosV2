using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.RepositoryBase;

namespace KiraStudios.Domain.IdentityRepository
{
    public interface IUserClaimRepository : IRepositoryBase<int, UserClaim> { }
}
