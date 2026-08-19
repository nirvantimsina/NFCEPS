using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NFCEPS.Application.Helpers;

namespace NFCEPS.Presentation.Helpers;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HasPermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _permKey;

    public HasPermissionAttribute(string permKey)
    {
        _permKey = permKey;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permService = context.HttpContext.RequestServices
            .GetRequiredService<PermissionService>();

        var roleIdClaim = context.HttpContext.User
            .FindFirst("roleId")?.Value;

        if (roleIdClaim == null ||
            !int.TryParse(roleIdClaim.ToString(), out var roleId) ||
            !permService.Has(roleId, _permKey))
        {
            context.Result = new ForbidResult();
        }
    }
}





