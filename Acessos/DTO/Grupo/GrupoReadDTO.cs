using Acessos.Models;

namespace Acessos.DTO.Grupo;

public class GrupoReadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ICollection<UsuarioGrupo> usuarioGrupos { get; set; }
}
