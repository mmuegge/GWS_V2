using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Dtos.Gas
{
    public class GasCounterCreateDto
  {
    [Required]
    public int? ID_Anbieter { get; set; }
    [NotMapped]
    public string? Anbieter { get; set; }
    [Required]
    public DateTime? Ablesetag { get; set; }
    [Required]
    public double? Zaehlerstand { get; set; }
    [Required]
    public DateTime? Uhrzeit { get; set; }
    [Required]
    public double? Temperatur_aussen { get; set; }
    [Required]
    public double? Temperatur_innen { get; set; }
    [MaxLength(250)]
    public string? Bemerkungen { get; set; }
  }
}