namespace GWS_Statistics.Data
{
    public class ParameterModel : IParameter
    {
        public int Id { get; set; }
        public DateTime? Baujahr { get; set; }
        public double? Wohnflaeche { get; set; }
        public string? Bemerkung { get; set; }
    }
}
