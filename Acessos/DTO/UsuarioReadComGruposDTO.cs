using Acessos.Models;
using System.Text.Json.Serialization;

namespace Acessos.DTO;

public class UsuarioReadComGruposDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public ICollection<UsuarioGrupoReadDTO> Grupos { get; set; }
}
