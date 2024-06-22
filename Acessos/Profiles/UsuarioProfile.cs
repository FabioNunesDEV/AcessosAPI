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
        CreateMap<Usuario, UsuarioReadDTO>();
        CreateMap<Usuario, UsuarioUpdateDTO>();        
    }
}
