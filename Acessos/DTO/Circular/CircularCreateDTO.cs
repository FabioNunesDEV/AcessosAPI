using System;
using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.Circular;

public class CircularCreateDTO
{
    [Required(ErrorMessage = "O assunto � obrigat�rio.")]
    [MaxLength(300, ErrorMessage = "O assunto deve ter no m�ximo 300 caracteres.")]
    public string Assunto { get; set; }

    [Required(ErrorMessage = "O conte�do � obrigat�rio.")]
    [MaxLength(3000, ErrorMessage = "O texto de conte�do deve ter no m�ximo 3000 caracteres.")]
    public string Conteudo { get; set; }
}

