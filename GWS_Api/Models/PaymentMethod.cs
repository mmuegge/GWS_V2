using System.ComponentModel.DataAnnotations;

namespace GWS_Api.Models
{
    public class PaymentMethod
    {
        [Key]
        public int ID_Zahlungsart { get; set; }
        [Required]
        public string? Zahlungsart { get; set; }
    }
}
