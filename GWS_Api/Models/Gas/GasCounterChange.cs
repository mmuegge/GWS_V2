using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Gas
{
    [Table("gas_zaehlerwechsel")]
    public class GasCounterChange
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        public int? Id_Anbieter { get; set; }

        [NotMapped]
        public string? Anbieter { get; set; } = string.Empty;

        [Required]
        public DateTime? Wechsel_Datum { get; set; }

        public double? Zaehlerstand_alt { get; set; }

        public double? Zaehlerstand_neu { get; set; }

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}
