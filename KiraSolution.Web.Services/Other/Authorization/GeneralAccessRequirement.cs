using KiraSolution.Web.Services.Other.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KiraSolution.Web.Services.Other.Authorization
{
    public class GeneralAccessRequirement : AuthorizationHandler<GeneralAccessRequirement>,
        IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, GeneralAccessRequirement requirement)
        {
            if (!context.User.Claims.Any()) return Task.CompletedTask;

            string controller = GetController(context);
            string roles = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;


            if (CheckForPermission(controller, roles, context)) context.Succeed(requirement);
            return Task.CompletedTask;
        }

        private bool CheckForPermission(string controller, string roles, AuthorizationHandlerContext context)
        {
            bool regresa = false;

            foreach (var rol in roles.Split('|'))
            {
                regresa = AppConfiguration.RoleControls.Any(rc =>
                            rc.Control.Name == controller &&
                            rc.Role.Name == rol);

                if (regresa) break;
            }

            return regresa;
        }

        private string GetController(AuthorizationHandlerContext context)
        {
            var endPoint = context.Resource as RouteEndpoint;
            var result = endPoint.RoutePattern.RawText;
            if (result.Contains("/")) result = result.Split('/')[0];
            return result;
        }
    }
}
