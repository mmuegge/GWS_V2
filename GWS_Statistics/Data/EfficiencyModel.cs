
namespace GWS_Statistics.Data
{
    public class EfficiencyModel : IEffiency
    {
        public int Id { get; set; }
        public string? Energieklasse { get; set; }
        public int? Energiebedarf { get; set; }
        public string? Farbcode { get; set; }
        public string? Bemerkung { get; set; }
    }
}
