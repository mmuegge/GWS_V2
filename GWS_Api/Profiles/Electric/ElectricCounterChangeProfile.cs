using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;

namespace GWS_Api.Profiles.Electric
{
    public class ElectricCounterChangeProfile : Profile
    {
        // für Automapper
        public ElectricCounterChangeProfile()
        {
            // source --> target
            CreateMap<ElectricCounterChange, ElectricCounterChangeReadDto>();
            CreateMap<ElectricCounterChangeCreateDto, ElectricCounterChange>();
            CreateMap<ElectricCounterChangeUpdateDto, ElectricCounterChange>();
            CreateMap<ElectricCounterChange, ElectricCounterChangeUpdateDto>();     // für patch-verb
        }
    }
}
