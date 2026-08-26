using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Dtos
{
    public class EfficiencyCreateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Energieklasse { get; set; }
        public int Energiebedarf { get; set; }
        public string? Farbcode { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
