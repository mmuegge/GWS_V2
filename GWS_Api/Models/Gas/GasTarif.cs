using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Gas
{
    [Table("gas_tarif")]
    public class GasTarif
    {
        [Key]
        [Column("ID_Anbieter")]                     // so heisst Spalte in DB
        public int Id { get; set; }                 // im Programm wird die Spalte "Id" genannt
        
        [MaxLength(100)]
        public string? Anbieter { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Tarif { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte eine Zählernummer angeben.")]
        [MaxLength(50)]
        public string? Zaehlernummer { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Kuendigungsfrist { get; set; } = string.Empty;

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

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}