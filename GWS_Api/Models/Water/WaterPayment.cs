using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Water
{
    public class WaterPayment
    {
        [Key]
        public int ID_Zahlung { get; set; }
        [Required]
        public int ID_Anbieter { get; set; }
        [NotMapped]
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
