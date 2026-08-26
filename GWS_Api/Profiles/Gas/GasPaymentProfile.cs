using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models.Gas;

namespace GWS_Api.Profiles.Gas
{
    public class GasPaymentProfile : Profile
    {
        // für Automapper
        public GasPaymentProfile()
        {
            // source --> target
            CreateMap<GasPayment, GasPaymentReadDto>();
            CreateMap<GasPaymentCreateDto, GasPayment>();
            CreateMap<GasPaymentUpdateDto, GasPayment>();
            CreateMap<GasPayment, GasPaymentUpdateDto>();     // für patch-verb
        }
    }
}
