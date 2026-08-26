namespace GWS_Api.Dtos
{
    public class ParameterReadDto
    {
        public int Id { get; set; }
        public DateTime? Baujahr { get; set; }
        public double Wohnflaeche { get; set; }
        public string? Bemerkung { get; set; }
    }
}
