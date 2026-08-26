using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;

namespace GWS_Api.Profiles.Electric
{
    public class ElectricCounterProfile : Profile
  {
    // für Automapper
    public ElectricCounterProfile()
    {
      // source --> target
      CreateMap<ElectricCounter, ElectricCounterReadDto>();
      CreateMap<ElectricCounterCreateDto, ElectricCounter>();
      CreateMap<ElectricCounterUpdateDto, ElectricCounter>();
      CreateMap<ElectricCounter, ElectricCounterUpdateDto>();     // für patch-verb
    }
  }
}