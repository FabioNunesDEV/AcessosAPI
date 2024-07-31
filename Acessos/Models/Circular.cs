using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acessos.Models;

[Table("Circular")]
public class Circular
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Protocolo {  get; set; }

    [Required]
    [Column(TypeName ="nvarchar(300)")]
    public string Assunto { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(3000)")]
    public string Conteudo { get; set; }

    [Required]
    public DateTime DataEnvio { get; set; }
    public DateTime? DataRecebimento { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public string Status { get; set; }
}
