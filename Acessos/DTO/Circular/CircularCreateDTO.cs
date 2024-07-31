using System;
using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Circular;

public class CircularCreateDTO
{
    [Required(ErrorMessage = "O assunto é obrigatório.")]
    [MaxLength(300, ErrorMessage = "O assunto deve ter no máximo 300 caracteres.")]
    public string Assunto { get; set; }

    [Required(ErrorMessage = "O conteúdo é obrigatório.")]
    [MaxLength(3000, ErrorMessage = "O texto de conteúdo deve ter no máximo 3000 caracteres.")]
    public string Conteudo { get; set; }
}

