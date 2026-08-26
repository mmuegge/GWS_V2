namespace GWS_Api.Dtos
{
    public class EfficiencyReadDto
    {
        public int Id { get; set; }
        public string? Energieklasse { get; set; }
        public int Energiebedarf { get; set; }
        public string? Farbcode{ get; set; }
        public string? Bemerkung { get; set; }
    }
}
