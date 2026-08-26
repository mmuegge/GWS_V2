using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Electric
{
    public class ElectricPaymentMethodUpdateDto
    {
        [Required]
        public int ID_Zahlungsart { get; set; }
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
