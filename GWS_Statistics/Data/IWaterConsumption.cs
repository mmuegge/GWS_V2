namespace GWS_Statistics.Data
{
    public interface IWaterConsumption
    {
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }
        public double? Consumption { get; set; }
        public double? ConsumptionOutside { get; set; }
        public double? Temperature { get; set; }
    }
}
