using Identity.Authorize.ClaimRequired;
using Identity.Authorize.PolicyRequired;
using Identity.Authorize.RolesRequired;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class TesteController : ControllerBase
{
    [HttpGet("policy")]
    [Authorize(Policy = Policies.HorarioComercial)]
    public string MensagemPolicy() {
        return "Voce está autenticado e está no horario comercial";
    }

    [HttpGet("role")]
    [Authorize(Roles = Roles.Admin)]
    public string MensagemRole()
    {
        return "Voce é um Admin e está autenticado";
    }

    [HttpGet("claim")]
    [ClaimsAuthrize(Claims.Teste, "Obter")]
    public string MensagemClaim()
    {
        return "você está autenticado e possui a claim para ler está msg";
    }
}