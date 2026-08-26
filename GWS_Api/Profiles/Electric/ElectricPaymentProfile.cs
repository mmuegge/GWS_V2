using AutoMapper;
using GWS_Api.Dtos.Electric;
using GWS_Api.Models.Electric;

namespace GWS_Api.Profiles.Electric
{
    public class ElectricPaymentProfile : Profile
    {
        // für Automapper
        public ElectricPaymentProfile()
        {
            // source --> target
            CreateMap<ElectricPayment, ElectricPaymentReadDto>();
            CreateMap<ElectricPaymentCreateDto, ElectricPayment>();
            CreateMap<ElectricPaymentUpdateDto, ElectricPayment>();
            CreateMap<ElectricPayment, ElectricPaymentUpdateDto>();     // für patch-verb
        }
    }
}
