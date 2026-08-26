namespace GWS_Api.Dtos.Water
{
    public class WaterCostReadDto
    {
        public int Id { get; set; }
        public int Id_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime? Gueltig_Ab { get; set; }
        public double? Grundpreis { get; set; }
        public double? Trinkwasserpreis { get; set; }
        public double? Abwasserpreis { get; set; }
        public double? Zaehlermiete { get; set; }
        public string? Bemerkung { get; set; }
    }
}
