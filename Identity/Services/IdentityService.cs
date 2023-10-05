using AutoMapper;
using Identity.Data.DTOs;
using Identity.Interface;
using Identity.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly IGerarTokenService _geraTokenService;
    private readonly IMapper _mapper;

    public IdentityService(
        SignInManager<Usuario> signInManager,
        UserManager<Usuario> userManager,
        IMapper mapper,
        IGerarTokenService geraTokenService)
    {
        this._signInManager = signInManager;
        this._userManager = userManager;
        this._mapper = mapper;
        _geraTokenService = geraTokenService;
    }

    public async Task<UsuarioCadastroResponse> CadastraUsuario(UsuarioCadastroRequest usuarioCadastroRequest)
    {
        var usuario = _mapper.Map<Usuario>(usuarioCadastroRequest);
        usuario.EmailConfirmed = true;

        var result = await this._userManager.CreateAsync(usuario, usuarioCadastroRequest.Senha ?? "");

        if (result.Succeeded)
            await this._userManager.SetLockoutEnabledAsync(usuario, false);

        var response = new UsuarioCadastroResponse(result.Succeeded);
        if (!result.Succeeded && result.Errors.Count() > 0)
        {
            response.AdicionarErros(result.Errors.Select(x => x.Description));
        }

        return response;
    }

    public async Task<UsuarioLoginResponse> Login(UsuarioLoginRequest usuarioLoginRequest)
    {
        var result = await this._signInManager.PasswordSignInAsync(usuarioLoginRequest.Nome, usuarioLoginRequest.Senha, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(usuarioLoginRequest.Nome);
            return await this._geraTokenService.Gerar(user ?? throw new Exception("Usuario não encontradado"));
        }

        var response = new UsuarioLoginResponse(result.Succeeded);
        if (!result.Succeeded)
        {
            if(result.IsLockedOut)
            {
                response.AdicionarError("Usuário está bloqueado");
            }else if (result.IsNotAllowed)
            {
                response.AdicionarError("Usuário não tem permissão para fazer login");
            }else if (result.RequiresTwoFactor)
            {
                response.AdicionarError("É necessário confirma sua senha no segundo fator de altenticação");
            }
            else {
                response.AdicionarError("Usuário ou senha estão incorretos");
            }
        }

        return response;
    }
}
