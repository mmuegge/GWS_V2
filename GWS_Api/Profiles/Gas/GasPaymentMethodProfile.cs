using AutoMapper;
using GWS_Api.Dtos.Gas;
using GWS_Api.Models;

namespace GWS_Api.Profiles.Gas
{
    public class GasPaymentMethodProfile: Profile
    {
        // für Automapper
        public GasPaymentMethodProfile()
        {
            // source --> target
            CreateMap<PaymentMethod, GasPaymentMethodReadDto>();
            CreateMap<GasPaymentMethodCreateDto, PaymentMethod>();
            CreateMap<GasPaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, GasPaymentMethodUpdateDto>();     // für patch-verb
        }
    }
}
