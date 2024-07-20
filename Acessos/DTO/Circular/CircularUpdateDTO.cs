using System;
using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Circular;

public class CircularUpdateDTO
{
    [MaxLength(300)]
    public string Assunto { get; set; }
    [MaxLength(3000)]
    public string Conteudo { get; set; }
}

