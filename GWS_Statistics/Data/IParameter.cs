
namespace GWS_Statistics.Data
{
    public interface IParameter
    {
        public int Id { get; set; }
        public DateTime? Baujahr { get; set; }
        public double? Wohnflaeche { get; set; }
        public string? Bemerkung { get; set; }
    }
}
