using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Auth;

public class AuthReadDTO
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Senha { get; set; }
}
