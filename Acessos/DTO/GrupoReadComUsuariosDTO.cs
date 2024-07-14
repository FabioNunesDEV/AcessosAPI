using Acessos.Models;

namespace Acessos.DTO;

public class GrupoReadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ICollection<UsuarioGrupo> usuarioGrupos { get; set; }
}
