using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;

namespace GWS_Api.Profiles.Gas
{
  public class GasCounterProfile : Profile
  {
    // für Automapper
    public GasCounterProfile()
    {
      // source --> target
      CreateMap<GasCounter, GasCounterReadDto>();
      CreateMap<GasCounterCreateDto, GasCounter>();
      CreateMap<GasCounterUpdateDto, GasCounter>();
      CreateMap<GasCounter, GasCounterUpdateDto>();     // für patch-verb
    }
  }
}