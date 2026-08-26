using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasPaymentMethodCreateDto
    {
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
