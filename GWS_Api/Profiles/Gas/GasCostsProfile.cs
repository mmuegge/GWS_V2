using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;

namespace GWS_Api.Profiles.Gas
{
    public class GasCostsProfile : Profile
    {
        // für Automapper
        public GasCostsProfile()
        {
            // source --> target
            CreateMap<GasCost, GasCostReadDto>();
            CreateMap<GasCostCreateDto, GasCost>();
            CreateMap<GasCostUpdateDto, GasCost>();
            CreateMap<GasCost, GasCostUpdateDto>();     // für patch-verb
        }
    }
}
