using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Dtos.Water
{
    public class WaterCostCreateDto
    {
        //[Required]
        //public int Id { get; set; }
        [Required]
        public int? Id_Anbieter { get; set; }
        [NotMapped]
        public string? Anbieter { get; set; }
        [Required]
        public DateTime? Gueltig_Ab { get; set; }
        public double? Grundpreis { get; set; }
        public double? Trinkwasserpreis { get; set; }
        public double? Abwasserpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
