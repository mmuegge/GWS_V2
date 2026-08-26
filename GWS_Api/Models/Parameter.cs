using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Models
{
    public class Parameter
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Baujahr { get; set; }
        public double Wohnflaeche { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
