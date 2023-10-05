using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Model;

public class Usuario : IdentityUser{
    [Required]
    [MinLength(3)]
    public string? Sobrenome { get; set; }
}
