using Acessos.Models;

namespace Acessos.DTO.Grupo;

public class GrupoReadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool PodeCriar { get; set; }
    public bool PodeLer { get; set; }
    public bool PodeAlterar { get; set; }
    public bool PodeDeletar { get; set; }
}
