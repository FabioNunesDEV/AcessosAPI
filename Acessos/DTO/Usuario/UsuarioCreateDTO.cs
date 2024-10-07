using System.ComponentModel.DataAnnotations;

namespace Acessos.DTO.usuario;

public class UsuarioCreateDTO
{
    [Required(ErrorMessage = "Campo nome obrigatório.")]
    [MaxLength(80, ErrorMessage = "Nome com no máximo 80 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo login obrigatório.")]
    [RegularExpression(@"^[a-z0-9.]+$", ErrorMessage = "Login deve conter apenas letras minúsculas, números e pontos.")]
    [MaxLength(30, ErrorMessage = "Login com no máximo 30 caracteres.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Campo email obrigatório.")]
    [MaxLength(80, ErrorMessage = "Email com no máximo caracteres 80.")]
    [EmailAddress(ErrorMessage = "Email informado não é válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo senha obrigatório.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "Campo confirmação de senha obrigatório.")]
    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmacaoSenha { get; set; }
}
