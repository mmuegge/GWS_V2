using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Models
{
    public class Efficiency
    {
        [Key]
        public int Id { get; set; }
        public string? Energieklasse { get; set; }
        public int Energiebedarf { get; set; }
        public string? Farbcode { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
