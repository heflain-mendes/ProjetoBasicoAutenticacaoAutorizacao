using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Authorize.PolicyRequired
{
    internal class HorarioComercialHandler : AuthorizationHandler<HorarioComercialRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HorarioComercialRequirement requirement)
        {
            var horarioAtual = TimeOnly.FromDateTime(DateTime.Now);
            if (horarioAtual.Hour >= 8 && horarioAtual.Hour <= 16)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
