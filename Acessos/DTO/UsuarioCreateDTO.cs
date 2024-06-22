using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO;

public class UsuarioCreateDTO
{
    [Required(ErrorMessage ="Campo nome obrigatório.")]
    [MaxLength(80,ErrorMessage ="Nome com no máximo 80 caracteres.") ]
    public string Nome { get; set; }

    [Required(ErrorMessage ="Campo email obrigatório.")]
    [MaxLength(80, ErrorMessage ="Email com no máximo caracteres 80.")]
    [EmailAddress(ErrorMessage = "Email informado não é válido.")]
    public string Email { get; set; }

}
