namespace GWS_Statistics.Data
{
    public class Water_SupplierDataModel
    {
        public int Id { get; set; }
        public string? Anbieter { get; set; }
        public DateTime? Zeitraum_Start { get; set; }
        public DateTime? Zeitraum_Ende { get; set; }
        public double? Start_Zaehlerstand { get; set; }
        public double? Ende_Zaehlerstand { get; set; }
    }
}
