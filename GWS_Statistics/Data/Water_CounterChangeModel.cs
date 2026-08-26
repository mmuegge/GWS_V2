namespace GWS_Statistics.Data
{
    public class Water_CounterChangeModel : ICounterChange
    {
        public int Id { get; set; }
        public int Id_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime Wechsel_Datum { get; set; }
        public double? Zaehlerstand_alt { get; set; }
        public double? Zaehlerstand_neu { get; set; }
        public double? Zaehlerstand_aussen_alt { get; set; }
        public double? Zaehlerstand_aussen_neu { get; set; }
        public string? Bemerkung { get; set; }
    }
}
