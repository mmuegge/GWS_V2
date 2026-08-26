using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos.Gas
{
    public class GasCounterUpdateDto
  {
    [Required]
    public int ID_Anbieter { get; set; }
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
  }
}