using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Water
{
    [Table("wasser_kosten")]
    public class WaterCost
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        public int Id_Anbieter { get; set; }

        [NotMapped]
        public string? Anbieter { get; set; } = string.Empty;

        [Required]
        public DateTime? Gueltig_Ab { get; set; }

        public double? Grundpreis { get; set; }

        public double? Trinkwasserpreis { get; set; }

        public double? Abwasserpreis { get; set; }

        public double? Zaehlermiete { get; set; }

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}
