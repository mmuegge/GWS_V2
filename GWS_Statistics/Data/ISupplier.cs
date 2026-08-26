namespace GWS_Statistics.Data
{
    public interface ISupplier
    {
        public int Id { get; set; }                 // im Programm wird die Spalte "Id" genannt
        public string? Anbieter { get; set; }
        public string? Tarif { get; set; }
        public string? Zaehlernummer { get; set; }
        public string? Kuendigungsfrist { get; set; }
        public DateTime? Zeitraum_Start { get; set; }
        public DateTime? Zeitraum_Ende { get; set; }
        public double? Start_Zaehlerstand { get; set; }
        public double? Ende_Zaehlerstand { get; set; }
        public double? Grundpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        public string? Bemerkung { get; set; }
    }
}
