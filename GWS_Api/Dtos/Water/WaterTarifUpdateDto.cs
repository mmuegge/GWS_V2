using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Water
{
    public class WaterTarifUpdateDto
    {
        public string? Anbieter { get; set; }
        public string? Tarif { get; set; }
        [Required]
        public string? Zaehlernummer { get; set; }
        public string? Kuendigungsfrist { get; set; }
        public DateTime? Zeitraum_Start { get; set; }
        public DateTime? Zeitraum_Ende { get; set; }
        public double? Start_Zaehlerstand { get; set; }
        public double? Ende_Zaehlerstand { get; set; }
        public double? Start_Zaehlerstand_aussen { get; set; }
        public double? Ende_Zaehlerstand_aussen { get; set; }
        public double? Trinkwasserpreis { get; set; }
        public double? Verbrauch_Trinkwasser { get; set; }
        public double? Abwasserpreis { get; set; }
        public double? Verbrauch_Abwasser { get; set; }
        public double? Grundpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        // hier können Eigenschaften ausgeblendet werden, sie werden in im ReadDto nicht mehr berücksichtigt
        //public string Bemerkung { get; set; }
        public string? Bemerkung { get; set; }
    }
}