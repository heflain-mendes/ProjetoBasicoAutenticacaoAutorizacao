using Identity.Data.DTOs;
using Identity.Interface;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private IIdentityService _identityService { get; set; }

    public UsuarioController(IIdentityService identityService)
    {
        this._identityService = identityService;
    }

    [HttpPost("cadastro")]
    public async Task<ActionResult<UsuarioCadastroResponse>> Cadastra(UsuarioCadastroRequest request)
    {
        //Vendo se o objeto passado não viola as restrições do UsuarioCadastroRequest
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _identityService.CadastraUsuario(request);

        if (resultado.Sucesso)
            return Ok(resultado);
        else if(resultado.Erros.Count > 0)
            return BadRequest(resultado);

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UsuarioLoginResponse>> Login(UsuarioLoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _identityService.Login(request);
        if (resultado.Sucesso)
            return Ok(resultado);
        else if (resultado.Erros.Count > 0)
            return BadRequest(resultado);

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}