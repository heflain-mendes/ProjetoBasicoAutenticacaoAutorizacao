using System.ComponentModel.DataAnnotations;

namespace Identity.Data.DTOs;

public class UsuarioCadastroRequest
{
    [Required]
    [MinLength(3)]
    public string? Nome { get; set; }

    [Required]
    [MinLength(3)]
    public string? Sobrenome { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }

    [DataType(DataType.EmailAddress)]
    [Required]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string? Telefone { get; set; }
}
