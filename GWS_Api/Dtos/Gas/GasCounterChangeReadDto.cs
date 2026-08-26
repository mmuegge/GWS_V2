namespace GWS_Api.Dtos.Gas
{
    public class GasCounterChangeReadDto
    {
        public int Id { get; set; }
        public int Id_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime? Wechsel_Datum { get; set; }
        public double? Zaehlerstand_alt { get; set; }
        public double? Zaehlerstand_neu { get; set; }
        public string? Bemerkung { get; set; }
    }
}
