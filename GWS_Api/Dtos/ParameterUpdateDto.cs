using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos
{
    public class ParameterUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime? Baujahr { get; set; }
        [Required]
        public double Wohnflaeche { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }

    }
}
