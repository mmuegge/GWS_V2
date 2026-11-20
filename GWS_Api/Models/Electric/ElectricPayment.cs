using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Electric
{
    [Table("strom_zahlungen")]
    public class ElectricPayment
    {
        [Key]
        [Column("ID_Zahlung")]
        public int ID_Zahlung { get; set; }

        public int ID_Anbieter { get; set; }

        [NotMapped]
        public string? Anbieter { get; set; } = string.Empty;

        [Required]
        public DateTime? Datum { get; set; }

        [Required]
        public string? Zahlungsart { get; set; }

        [Required]
        public double? Zahlungen { get; set; }

        [MaxLength(250)]
        public string? Bemerkungen { get; set; } = string.Empty;
    }
}
