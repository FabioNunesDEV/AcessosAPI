using AutoMapper;
using Acessos.DTO.Circular;
using Acessos.Models;

namespace Acessos.Profiles;

public class CircularProfile : Profile
{
    public CircularProfile()
    {
        CreateMap<Circular, CircularReadDTO>();
        CreateMap<CircularCreateDTO, Circular>();
        CreateMap<CircularUpdateDTO, Circular>();
    }
}

