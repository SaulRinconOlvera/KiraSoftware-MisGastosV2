using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class UserClaimRepository : RepositoryBase<int, UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
