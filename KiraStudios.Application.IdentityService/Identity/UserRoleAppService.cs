using KiraStudios.Application.ServiceBase;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityRepository;
using KiraSudios.Application.IdentityViewModel.Identity;

namespace KiraStudios.Application.IdentityService.Identity
{
    public class UserRoleAppService :
        ApplicationServiceBase<UserRole, UserRoleViewModel>, IUserRoleAppService
    {
        public UserRoleAppService(IUserRoleRepository repository)
        { _repository = repository; }
    }
}
