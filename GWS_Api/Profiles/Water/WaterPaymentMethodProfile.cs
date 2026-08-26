using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models;

namespace GWS_Api.Profiles.Water
{
    public class WaterPaymentMethodProfile: Profile
    {
        // für Automapper
        public WaterPaymentMethodProfile()
        {
            // source --> target
            CreateMap<PaymentMethod, WaterPaymentMethodReadDto>();
            CreateMap<WaterPaymentMethodCreateDto, PaymentMethod>();
            CreateMap<WaterPaymentMethodUpdateDto, PaymentMethod>();
            CreateMap<PaymentMethod, WaterPaymentMethodUpdateDto>();     // für patch-verb
        }
    }
}
