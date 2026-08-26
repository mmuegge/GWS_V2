using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Electric
{
    public class ElectricPaymentUpdateDto
    {
        [Required]
        public int ID_Zahlung { get; set; }
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
