using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO;

public class GrupoUpdateDTO
{
    [Required(ErrorMessage = "Campo nome obrigatório.")]
    [MaxLength(80, ErrorMessage = "Nome com no máximo 80 caracteres.")]
    public string Nome { get; set; }
}
