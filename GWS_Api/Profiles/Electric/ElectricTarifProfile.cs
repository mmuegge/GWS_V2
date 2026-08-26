using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models;
using GWS_Api.Models.Electric;

namespace GWS_Api.Profiles.Electric
{
  public class ElectricTarifProfile : Profile
  {
    // für Automapper
    public ElectricTarifProfile()
    {
      // source --> target
      CreateMap<ElectricTarif, ElectricTarifReadDto>();
      CreateMap<ElectricTarifCreateDto, ElectricTarif>();
      CreateMap<ElectricTarifUpdateDto, ElectricTarif>();
      CreateMap<ElectricTarif, ElectricTarifUpdateDto>();     // für patch-verb
    }
  }

}