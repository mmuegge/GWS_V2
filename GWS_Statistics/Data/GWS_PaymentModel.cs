using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GWS_Statistics.Data
{
    public class GWS_PaymentModel: IPayments
    {
        public int ID_Zahlung { get; set; }                 
        public int ID_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime Datum { get; set; }
        public string? Zahlungsart { get; set; }
        public double? Zahlungen { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
