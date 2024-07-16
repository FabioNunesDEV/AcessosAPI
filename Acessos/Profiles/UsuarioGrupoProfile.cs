using Acessos.DTO.Relacionamento;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Profiles
{
    public class UsuarioGrupoProfile : Profile
    {
        public UsuarioGrupoProfile()
        {
            CreateMap<UsuarioGrupoCreateDTO, UsuarioGrupo>();
            CreateMap<UsuarioGrupo, UsuarioGrupoReadDTO>();
        }
    }
}
