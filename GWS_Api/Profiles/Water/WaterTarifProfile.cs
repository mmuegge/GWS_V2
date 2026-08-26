using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;

namespace GWS_Api.Profiles.Water
{
  public class WaterTarifProfile : Profile
  {
    // für Automapper
    public WaterTarifProfile()
    {
      // source --> target
      CreateMap<WaterTarif, WaterTarifReadDto>();
      CreateMap<WaterTarifCreateDto, WaterTarif>();
      CreateMap<WaterTarifUpdateDto, WaterTarif>();
      CreateMap<WaterTarif, WaterTarifUpdateDto>();     // für patch-verb
    }
  }

}