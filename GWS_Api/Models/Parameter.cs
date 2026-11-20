using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models
{
    [Table("haus_parameter")]
    public class Parameter
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public DateTime? Baujahr { get; set; }

        public double Wohnflaeche { get; set; }

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}
