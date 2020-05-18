using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.RepositoryBase;
using System.Threading.Tasks;

namespace KiraStudios.Domain.IdentityRepository
{
    public interface IUserRepository : IRepositoryBase<int, User>
    {
        Task<User> LoginAsync(User entity);
        Task<User> SocialNetwiorkLoginAsync(string userId, string platform);
        Task LogoutAsync();
    }
}
