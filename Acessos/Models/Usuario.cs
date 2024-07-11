using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acessos.Models;

[Table("Usuario")]
public class Usuario
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(80)]
    [EmailAddress]
    public string Email { get; set; }

    public virtual ICollection<UsuarioGrupo> UsuarioGrupos { get; set; }
}
