namespace GWS_Statistics.Data
{
    public interface IPaymentConsumption
    {
        public DateTime? Date { get; set; }
        public double Payment { get; set; }
        public double Consumption { get; set; }
    }
}
