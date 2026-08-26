using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Water
{
    public class WaterPaymentMethodCreateDto
    {
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
