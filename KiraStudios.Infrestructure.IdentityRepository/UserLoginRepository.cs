using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.RepositoryBase;

namespace KiraStudios.Infraestructure.IdentityRepository
{
    public class UserLoginRepository : RepositoryBase<int, UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }
    }
}
