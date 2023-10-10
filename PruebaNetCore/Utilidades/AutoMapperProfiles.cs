using AutoMapper;
using PruebaNetCore.Modelos;
using PruebaNetCore.Modelos.Dtos;

namespace PruebaNetCore.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Cliente, ClienteCreacionDto>().ReverseMap();
            CreateMap<Cuenta, CuentaDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoCreacionDto>().ReverseMap();
        }
    }
}
