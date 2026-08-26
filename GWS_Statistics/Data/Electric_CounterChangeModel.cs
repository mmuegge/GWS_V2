namespace GWS_Statistics.Data
{
    public class Electric_CounterChangeModel : ICounterChange
    {
        public int Id { get; set; }
        public int Id_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime Wechsel_Datum { get; set; }
        public double? Zaehlerstand_alt { get; set; }
        public double? Zaehlerstand_neu { get; set; }
        public double? Zaehlerstand_280_alt { get; set; }
        public double? Zaehlerstand_280_neu { get; set; }
        public double? Zaehlerstand_Enfluri_alt { get; set; }
        public double? Zaehlerstand_Enfluri_neu { get; set; }
        public string? Bemerkung { get; set; }
     
    }
}
