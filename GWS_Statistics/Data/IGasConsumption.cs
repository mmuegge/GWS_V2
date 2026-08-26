namespace GWS_Statistics.Data
{
    public interface IGasConsumption
    {
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }
        public double? Consumption { get; set; }
        public double? Temperature { get; set; }
    }
}
