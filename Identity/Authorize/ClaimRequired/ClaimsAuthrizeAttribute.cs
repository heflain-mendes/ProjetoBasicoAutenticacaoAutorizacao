using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Authorize.ClaimRequired;

public class ClaimsAuthrizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthrizeAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter)) =>
           Arguments = new object[] { new Claim(claimType, claimValue) };
}
