using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;

namespace GWS_Api.Profiles.Water
{
    public class WaterCounterChangeProfile : Profile
    {
        public WaterCounterChangeProfile()
        {
            // source --> target
            CreateMap<WaterCounterChange, WaterCounterChangeReadDto>();
            CreateMap<WaterCounterChangeCreateDto, WaterCounterChange>();
            CreateMap<WaterCounterChangeUpdateDto, WaterCounterChange>();
            CreateMap<WaterCounterChange, WaterCounterChangeUpdateDto>();     // für patch-verb
        }
    }
}
