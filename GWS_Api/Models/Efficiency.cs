using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models
{
    [Table("energie_effizienz")]
    public class Efficiency
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public string? Energieklasse { get; set; } = string.Empty;

        public int Energiebedarf { get; set; }

        public string? Farbcode { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Bemerkung { get; set; } = string.Empty;
    }
}
