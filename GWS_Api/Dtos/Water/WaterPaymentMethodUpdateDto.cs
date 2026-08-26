using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Water
{
    public class WaterPaymentMethodUpdateDto
    {
        [Required]
        public int ID_Zahlungsart { get; set; }
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
