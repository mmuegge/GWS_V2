using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasPaymentMethodUpdateDto
    {
        [Required]
        public int ID_Zahlungsart { get; set; }
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
