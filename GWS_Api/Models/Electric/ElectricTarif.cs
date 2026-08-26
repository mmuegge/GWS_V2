using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Electric
{
    public class ElectricTarif
    {
        [Column("ID_Anbieter")]                     // so heisst Spalte in DB
        public int Id { get; set; }                 // im Programm wird die Spalte "Id" genannt
        public string? Anbieter { get; set; }
        public string? Tarif { get; set; }
        [Required]
        public string? Zaehlernummer { get; set; }
        public string? Kuendigungsfrist { get; set; }
        public DateTime? Zeitraum_Start { get; set; }
        public DateTime? Zeitraum_Ende { get; set; }
        public double? Start_Zaehlerstand { get; set; }
        public double? Ende_Zaehlerstand { get; set; }
        public double? Start_Zaehlerstand_280 { get; set; }
        public double? Ende_Zaehlerstand_280 { get; set; }
        public double? Start_Zaehlerstand_Enfluri { get; set; }
        public double? Ende_Zaehlerstand_Enfluri { get; set; }
        public double? Arbeitspreis { get; set; }
        public double? Grundpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        public int? Anzahl_Personen { get; set; }
        public string? Bemerkung { get; set; }
    }
}