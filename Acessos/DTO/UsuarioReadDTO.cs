using Acessos.Models;

namespace Acessos.DTO;

public class UsuarioReadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

    public ICollection<UsuarioGrupo> usuarioGrupos { get; set; }
}
