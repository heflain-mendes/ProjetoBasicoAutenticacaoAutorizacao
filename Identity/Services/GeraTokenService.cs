using Identity.Configuration;
using Identity.Data.DTOs;
using Identity.Interface;
using Identity.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    
    public class GeraTokenService: IGerarTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<Usuario> _userManager;

        public GeraTokenService(IOptions<JwtOptions> jwtOptions, UserManager<Usuario> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<UsuarioLoginResponse> Gerar(Usuario usuario)
        {
            var acessTokenClaims = await ObterClaims(usuario, addClaimUsuario: true);

            var dataExpiracaoAcessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var acessToken = GerarToken(acessTokenClaims, dataExpiracaoAcessToken);

            return new UsuarioLoginResponse(
                sucesso: true,
                token: acessToken,
                DataExpiracao: dataExpiracaoAcessToken
                );
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<List<Claim>> ObterClaims(Usuario usuario, Boolean addClaimUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString())
            };

            if (addClaimUsuario)
            {
                var usuarioClaim = await this._userManager.GetClaimsAsync(usuario);
                var roles = await this._userManager.GetRolesAsync(usuario);

                claims.AddRange(usuarioClaim);

                foreach (var item in roles)
                {
                    claims.Add(new Claim("role", item.ToString()));
                }
            }

            return claims;
        }
    }
}
