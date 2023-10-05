using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Data.DTOs;

public class UsuarioLoginRequest
{

    [Required]
    public string Nome { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;
}
