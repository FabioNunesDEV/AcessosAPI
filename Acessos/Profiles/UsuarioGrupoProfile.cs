using Acessos.DTO;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Profiles
{
    public class UsuarioGrupoProfile : Profile
    {
        public UsuarioGrupoProfile()
        {
            CreateMap<UsuarioGrupoCreateDTO, UsuarioGrupo>();
            CreateMap<UsuarioGrupo, UsuarioGrupoReadDTO>()
                 .ForMember(dest => dest.GrupoNome, opt => opt.MapFrom(src => src.Grupo.Nome)); 
        }
    }
}
