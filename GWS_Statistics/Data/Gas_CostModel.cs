
namespace GWS_Statistics.Data
{
    public class Gas_CostModel : ICost
    {
        public int Id { get; set; }
        public int Id_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime Gueltig_Ab { get; set; }
        public double? Grundpreis { get; set; }
        public double? Arbeitspreis { get; set; }
        public double? Zaehlermiete { get; set; }
        public string? Bemerkung { get; set; }
    }
}
