using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models.Water
{
    public class WaterCounter
  {
    [Key]
    public int ID_Tag { get; set; }
    [Required]
    public int ID_Anbieter { get; set; }
    [NotMapped]
    public string? Anbieter { get; set; }
    [Required]
    public DateTime? Ablesetag { get; set; }
    [Required]
    public double? Zaehlerstand { get; set; }
    public DateTime? Uhrzeit { get; set; }
    public double? Temperatur_aussen { get; set; }
    public double? Temperatur_innen { get; set; }
    [MaxLength(250)]
    public string? Bemerkungen { get; set; }
    public double? Zaehlerstand_aussen { get; set; }
  }
}