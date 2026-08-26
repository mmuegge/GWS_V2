using System.ComponentModel.DataAnnotations;

namespace GWS_Statistics.Data
{
    public class Electric_SupplierModel : ISupplier
    {
        public int Id { get; set; }                 // im Programm wird die Spalte "Id" genannt
        public string? Anbieter { get; set; }
        public string? Tarif { get; set; }
        [Required]
        public string? Zaehlernummer { get; set; }
        public string? Kuendigungsfrist { get; set; }
        public DateTime? Zeitraum_Start { get; set; }
        public DateTime? Zeitraum_Ende { get; set; }
        public double? Start_Zaehlerstand { get; set; }  // Bezugszähler
        public double? Ende_Zaehlerstand { get; set; }   // Bezugszähler
        public double? Start_Zaehlerstand_280 { get; set; }  // Einspeisezähler
        public double? Ende_Zaehlerstand_280 { get; set; }   // Einspeisezähler
        public double? Start_Zaehlerstand_Enfluri { get; set; }  // Enfluri
        public double? Ende_Zaehlerstand_Enfluri { get; set; }   // Enfluri
        public double? Arbeitspreis { get; set; }
        public double? Grundpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        //public int? Anzahl_Personen { get; set; }
        public string? Bemerkung { get; set; }
    }
}
