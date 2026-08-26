namespace GWS_Api.Dtos.Gas
{
    public class GasBoilerReadDto
    {
        public int Id { get; set; }
        public DateTime? Verbrauchsjahr { get; set; }
        public double? Gesamt_Verbrauch { get; set; }
        public double? Heizung_Verbrauch { get; set; }
        public double? Warmwasser_Verbrauch { get; set; }
        public double? Strom_Verbrauch { get; set; }
        public string? Bemerkung { get; set; }
    }
}
