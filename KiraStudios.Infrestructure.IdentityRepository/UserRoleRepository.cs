using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class UserRoleRepository : RepositoryBase<int, UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
