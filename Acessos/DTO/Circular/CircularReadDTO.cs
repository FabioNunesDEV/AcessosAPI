using Acessos.Models;
using System;

namespace Acessos.DTO.Circular;

public class CircularReadDTO
{
    public int Id { get; set; }
    public string Protocolo { get; set; }
    public string Assunto { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataEnvio { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public string Status { get; set; }
}
