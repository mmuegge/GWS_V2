using System.Reflection.Metadata.Ecma335;

namespace GWS_Statistics.Data
{
    public class Water_PaymentConsumption : IPaymentConsumption
    {
        public DateTime? Date { get; set; }
        public double Payment { get; set; }
        public double Consumption{ get; set; }
        public double ConsumptionOutside { get; set; }
    }
}
