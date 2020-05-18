using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KiraSolutions.Infrastructure.Configuration
{
    public static class IdentityConfiguration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            UserManager = services.BuildServiceProvider().GetService<UserManager<User>>();
            SignManager = services.BuildServiceProvider().GetService<SignInManager<User>>();
            RoleManager = services.BuildServiceProvider().GetService<RoleManager<Role>>();
        }

        public static UserManager<User> UserManager { get; private set; }
        public static SignInManager<User> SignManager { get; private set; }
        public static RoleManager<Role> RoleManager { get; private set; }
    }
}
