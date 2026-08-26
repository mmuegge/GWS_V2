using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;

namespace GWS_Api.Profiles.Water
{
  public class WaterCounterProfile : Profile
  {
    // für Automapper
    public WaterCounterProfile()
    {
      // source --> target
      CreateMap<WaterCounter, WaterCounterReadDto>();
      CreateMap<WaterCounterCreateDto, WaterCounter>();
      CreateMap<WaterCounterUpdateDto, WaterCounter>();
      CreateMap<WaterCounter, WaterCounterUpdateDto>();     // für patch-verb
    }
  }
}