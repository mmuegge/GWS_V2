using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;

namespace GWS_Api.Profiles.Electric
{
    public class ElectricCostsProfile : Profile
    {
        // für Automapper
        public ElectricCostsProfile()
        {
            // source --> target
            CreateMap<ElectricCost, ElectricCostReadDto>();
            CreateMap<ElectricCostCreateDto, ElectricCost>();
            CreateMap<ElectricCostUpdateDto, ElectricCost>();
            CreateMap<ElectricCost, ElectricCostUpdateDto>();     // für patch-verb
        }
    }
}
