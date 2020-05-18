using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.RepositoryBase;

namespace KiraStudios.Domain.IdentityRepository
{
    public interface IUserLoginRepository : IRepositoryBase<int, UserLogin> { }
}
