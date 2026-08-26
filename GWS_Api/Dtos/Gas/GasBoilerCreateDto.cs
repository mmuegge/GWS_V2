using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasBoilerCreateDto
    {
        //[Required]
        //public int Id { get; set; }
        [Required]
        public DateTime Verbrauchsjahr { get; set; }
        public double? Gesamt_Verbrauch { get; set; }
        public double? Heizung_Verbrauch { get; set; }
        public double? Warmwasser_Verbrauch { get; set; }
        public double? Strom_Verbrauch { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
