using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acessos.Models;

[Table("Grupo")]
public class Grupo
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Nome { get; set; }
}