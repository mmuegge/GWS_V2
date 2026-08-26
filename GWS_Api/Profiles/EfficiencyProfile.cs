using AutoMapper;
using GWS_Api.Dtos;
using GWS_Api.Models;

namespace GWS_Api.Profiles
{
    public class EfficiencyProfile : Profile
    {
        public EfficiencyProfile()
        {
            // source --> target
            CreateMap<Efficiency, EfficiencyReadDto>();
            CreateMap<EfficiencyCreateDto, Efficiency>();
            CreateMap<EfficiencyUpdateDto, Efficiency>();
            CreateMap<Efficiency, EfficiencyUpdateDto>();     // für patch-verb
        }
    }
}
