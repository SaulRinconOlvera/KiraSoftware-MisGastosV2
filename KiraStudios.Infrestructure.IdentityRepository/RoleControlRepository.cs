using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class RoleControlRepository : RepositoryBase<int, RoleControl>, IRoleControlRepository
    {
        public RoleControlRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
