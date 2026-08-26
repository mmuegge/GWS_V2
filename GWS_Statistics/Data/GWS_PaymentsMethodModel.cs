using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GWS_Statistics.Data
{
    public class GWS_PaymentsMethodModel
    {
        public int ID_Zahlungsart { get; set; }                 
        public string? Zahlungsart { get; set; }
        
    }
}
