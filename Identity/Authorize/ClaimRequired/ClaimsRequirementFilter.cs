using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Authorize.ClaimRequired;

public class ClaimRequirementFilter : IAuthorizationFilter
{
    readonly Claim _claim;

    public ClaimRequirementFilter(Claim claim) =>
        _claim = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User as ClaimsPrincipal;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!user.HasClaim(_claim.Type, _claim.Value))
            context.Result = new ForbidResult();
    }
}
