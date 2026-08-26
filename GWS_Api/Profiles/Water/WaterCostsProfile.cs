using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;

namespace GWS_Api.Profiles.Water
{
    public class WaterCostsProfile : Profile
    {
        // für Automapper
        public WaterCostsProfile()
        {
            // source --> target
            CreateMap<WaterCost, WaterCostReadDto>();
            CreateMap<WaterCostCreateDto, WaterCost>();
            CreateMap<WaterCostUpdateDto, WaterCost>();
            CreateMap<WaterCost, WaterCostUpdateDto>();     // für patch-verb
        }
    }
}
