using Acessos.DTO;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Profiles;

public class GrupoProfile : Profile
{
    public GrupoProfile()
    {
        CreateMap<GrupoCreateDTO, Grupo>();
        CreateMap<GrupoUpdateDTO, Grupo>();
        CreateMap<Grupo, GrupoReadDTO>();
        CreateMap<Grupo, GrupoUpdateDTO>();
    }
}