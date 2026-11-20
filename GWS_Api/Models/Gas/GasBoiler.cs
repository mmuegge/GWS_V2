using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Gas
{
    [Table("gas_therme")]
    public class GasBoiler
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public DateTime? Verbrauchsjahr { get; set; }

        public double? Gesamt_Verbrauch { get; set; }

        public double? Heizung_Verbrauch { get; set; }

        public double? Warmwasser_Verbrauch { get; set; }

        public double? Strom_Verbrauch { get; set; }

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}
