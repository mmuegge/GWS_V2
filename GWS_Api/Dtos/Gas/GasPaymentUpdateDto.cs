using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasPaymentUpdateDto
    {
        [Required]
        public int ID_Anbieter { get; set; }
        [Required]
        public string? Anbieter { get; set; }
        [Required]
        public DateTime? Datum { get; set; }
        [Required]
        public string? Zahlungsart { get; set; }
        [Required]
        public double? Zahlungen { get; set; }
        [MaxLength(250)]
        public string? Bemerkungen { get; set; }
    }
}
