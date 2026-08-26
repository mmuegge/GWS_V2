namespace GWS_Statistics.Data
{
    public interface IElectricConsumption
    {
        public int SupplierId { get; set; }
        public DateTime Date { get; set; }
        public double? Consumption { get; set; }
        public double? Consumption_280 { get; set; }
        public double? Consumption_Enfluri { get; set; }
        public double? Temperature { get; set; }
    }
}
