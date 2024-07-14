using Acessos.Models;

namespace Acessos.DTO;

public class GrupoReadComUsuariosDTO:GrupoReadDTO
{
    public ICollection<UsuarioGrupo> usuarioGrupos { get; set; }
}
