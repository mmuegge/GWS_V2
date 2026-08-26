using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Electric
{
    public class ElectricPaymentMethodCreateDto
    {
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
