using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acessos.Models;

[Table("UsuarioGrupo")]
public class UsuarioGrupo
{
    [Required]    
    public int? UsuarioId { get; set; }
    public virtual Usuario Usuario { get; set; }

    [Required]
    public int? GrupoId { get; set; }
    public virtual Grupo Grupo { get; set; }
}
