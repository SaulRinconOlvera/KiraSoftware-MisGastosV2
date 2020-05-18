using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class RoleRepository : RepositoryBase<int, Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) { }
    }
}
