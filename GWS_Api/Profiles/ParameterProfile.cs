using AutoMapper;
using GWS_Api.Dtos;
using GWS_Api.Models;

namespace GWS_Api.Profiles
{
    public class ParameterProfile : Profile
    {
        public ParameterProfile()
        {
            // source --> target
            CreateMap<Parameter, ParameterReadDto>();
            CreateMap<ParameterCreateDto, Parameter>();
            CreateMap<ParameterUpdateDto, Parameter>();
            CreateMap<Parameter, ParameterUpdateDto>();     // für patch-verb
        }
    }
}
