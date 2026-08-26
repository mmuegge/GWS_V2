namespace GWS_Statistics.Data
{
    public interface IWaterCounter
    {
        public int ID_Tag { get; set; }
        public int ID_Anbieter { get; set; }
        public string? Anbieter { get; set; }
        public DateTime Ablesetag { get; set; }
        public double? Zaehlerstand { get; set; }
        public double? Zaehlerstand_aussen { get; set; }
        public DateTime? Uhrzeit { get; set; }
        public double? Temperatur_aussen { get; set; }
        public double? Temperatur_innen { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
