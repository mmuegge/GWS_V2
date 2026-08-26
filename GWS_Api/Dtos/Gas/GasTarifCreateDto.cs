using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasTarifCreateDto
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
        public double? Arbeitspreis { get; set; }
        public double? Grundpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        public double? Brennwert { get; set; }
        public double? Heizleistung { get; set; }
        public double? Zustandszahl { get; set; }
        public string? Bemerkung { get; set; }
    }
}