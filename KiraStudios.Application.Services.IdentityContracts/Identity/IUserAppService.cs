using KiraStudios.Application.ServiceBase;
using KiraSudios.Application.IdentityViewModel.Identity;
using System.Threading.Tasks;

namespace KiraStudios.Application.Services.IdentityContracts.Identity
{
    public interface IUserAppService : IApplicationServiceBase<int, UserViewModel>
    {
        Task<UserViewModel> LoginAsync(UserViewModel userViewModel);
        Task<UserViewModel> SocialNetwiorkLoginAsync(string userId, string platform);
        Task<UserViewModel> GetForModifyAsync(int viewModelId);
        UserViewModel GetForModify(int viewModelId);
        Task Logout();
    }
}
