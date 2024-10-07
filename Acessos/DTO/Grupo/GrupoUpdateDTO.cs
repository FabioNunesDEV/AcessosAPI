using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Grupo;

public class GrupoUpdateDTO
{
    [Required(ErrorMessage = "Campo nome obrigatório.")]
    [MaxLength(80, ErrorMessage = "Nome com no máximo 80 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo PodeCriar obrigatório.")]
    [RegularExpression("^(true|false)$", ErrorMessage = "Campo PodeCriar deve ser 'true' ou 'false'.")]
    public bool PodeCriar { get; set; }

    [Required]
    [RegularExpression("^(true|false)$", ErrorMessage = "Campo PodeLer deve ser 'true' ou 'false'.")]
    public bool PodeLer { get; set; }

    [Required]
    [RegularExpression("^(true|false)$", ErrorMessage = "Campo PodeAlterar deve ser 'true' ou 'false'.")]
    public bool PodeAlterar { get; set; }

    [Required]
    [RegularExpression("^(true|false)$", ErrorMessage = "Campo PodeDeletar deve ser 'true' ou 'false'.")]
    public bool PodeDeletar { get; set; }
}
