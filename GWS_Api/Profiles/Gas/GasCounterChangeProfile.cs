using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;

namespace GWS_Api.Profiles.Gas
{
    public class GasCounterChangeProfile : Profile
    {
        public GasCounterChangeProfile()
        {
            // source --> target
            CreateMap<GasCounterChange, GasCounterChangeReadDto>();
            CreateMap<GasCounterChangeCreateDto, GasCounterChange>();
            CreateMap<GasCounterChangeUpdateDto, GasCounterChange>();
            CreateMap<GasCounterChange, GasCounterChangeUpdateDto>();     // für patch-verb
        }
    }
}
