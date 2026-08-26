using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;
namespace GWS_Api.Profiles.Gas
{
    public class GasBoilerProfile : Profile
    {
        public GasBoilerProfile()
        {
            // source --> target
            CreateMap<GasBoiler, GasBoilerReadDto>();
            CreateMap<GasBoilerCreateDto, GasBoiler>();
            CreateMap<GasBoilerUpdateDto, GasBoiler>();
            CreateMap<GasBoiler, GasBoilerUpdateDto>();     // für patch-verb
        }
    }
}
