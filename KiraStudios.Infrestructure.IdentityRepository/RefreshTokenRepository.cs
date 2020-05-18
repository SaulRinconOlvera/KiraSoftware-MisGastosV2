using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class RefreshTokenRepository : RepositoryBase<int, RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) { }
    }
}
