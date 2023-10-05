using Identity.Configuration;
using Identity.Data.DTOs;
using Identity.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Interface;

public interface IGerarTokenService
{
    public Task<UsuarioLoginResponse> Gerar(Usuario usuario);

}
