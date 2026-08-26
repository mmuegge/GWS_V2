namespace GWS_Statistics.Data
{
    public class Gas_ConsumptionModel : IConsumption
    {
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }
        public double? Consumption { get; set; }
        public double? Consumption2 { get; set; }
        public double? Consumption3 { get; set; }
        public double? Temperature { get; set; }
    }
}
