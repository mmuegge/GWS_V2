using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models;

namespace GWS_Api.Profiles.Electric
{
    public class ElectricPaymentMethodProfile: Profile
    {
        // für Automapper
        public ElectricPaymentMethodProfile()
        {
            // source --> target
            CreateMap<PaymentMethod, ElectricPaymentMethodReadDto>();
            CreateMap<ElectricPaymentMethodCreateDto, PaymentMethod>();
            CreateMap<ElectricPaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, ElectricPaymentMethodUpdateDto>();     // für patch-verb
        }
    }
}
