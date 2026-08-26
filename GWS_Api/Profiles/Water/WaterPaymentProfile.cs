using AutoMapper;
using GWS_Api.Dtos.Water;
using GWS_Api.Models.Water;

namespace GWS_Api.Profiles.Water
{
    public class WaterPaymentProfile : Profile
    {
        // für Automapper
        public WaterPaymentProfile()
        {
            // source --> target
            CreateMap<WaterPayment, WaterPaymentReadDto>();
            CreateMap<WaterPaymentCreateDto, WaterPayment>();
            CreateMap<WaterPaymentUpdateDto, WaterPayment>();
            CreateMap<WaterPayment, WaterPaymentUpdateDto>();     // für patch-verb
        }
    }
}
