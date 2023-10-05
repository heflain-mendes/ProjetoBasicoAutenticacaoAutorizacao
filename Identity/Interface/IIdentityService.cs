using Identity.Data.DTOs;
namespace Identity.Interface;

public interface IIdentityService
{
    public Task<UsuarioCadastroResponse> CadastraUsuario(UsuarioCadastroRequest usuarioCadastroRequest);
    public Task<UsuarioLoginResponse> Login(UsuarioLoginRequest usuarioLoginRequest);

}
