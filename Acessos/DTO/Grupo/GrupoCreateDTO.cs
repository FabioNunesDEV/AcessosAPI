using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Grupo;

public class GrupoCreateDTO
{
    [Required(ErrorMessage = "Campo nome obrigatório.")]
    [MaxLength(80, ErrorMessage = "Nome com no máximo 80 caracteres.")]
    public string Nome { get; set; }
}
