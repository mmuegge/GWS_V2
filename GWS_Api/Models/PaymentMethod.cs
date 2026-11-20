using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Api.Models
{
    [Table("zahlungsarten")]
    public class PaymentMethod
    {
        [Key]
        [Column("ID_Zahlungsart")]
        public int ID_Zahlungsart { get; set; }
        [Required]
        public string? Zahlungsart { get; set; } = string.Empty;
    }
}
