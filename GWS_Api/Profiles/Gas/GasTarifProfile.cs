using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;

namespace GWS_Api.Profiles.Gas
{
  public class GasTarifProfile : Profile
  {
    // für Automapper
    public GasTarifProfile()
    {
      // source --> target
      CreateMap<GasTarif, GasTarifReadDto>();
      CreateMap<GasTarifCreateDto, GasTarif>();
      CreateMap<GasTarifUpdateDto, GasTarif>();
      CreateMap<GasTarif, GasTarifUpdateDto>();     // für patch-verb
    }
  }

}