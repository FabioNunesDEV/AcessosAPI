using Acessos.DTO;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<UsuarioCreateDTO,  Usuario>();
        CreateMap<UsuarioUpdateDTO, Usuario>();
        CreateMap<Usuario, UsuarioReadDTO>()
            .ForMember(UsurioDTO => UsurioDTO.usuarioGrupos, 
                       opt => opt.MapFrom(Usuario => Usuario.UsuarioGrupos)); 
        CreateMap<Usuario, UsuarioUpdateDTO>();        
    }
}
