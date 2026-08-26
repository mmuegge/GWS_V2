using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GWS_Statistics.Data
{
    public class Electric_CounterModel : ICounter
    {
        public int ID_Tag { get; set; }
        public int ID_Anbieter { get; set; }
        [NotMapped]
        public string? Anbieter { get; set; }
        public DateTime Ablesetag { get; set; }
        public double? Zaehlerstand { get; set; }
        //public double? Zaehlerstand2 { get; set; }
        //public double? Zaehlerstand3 { get; set; }
        public double? Zaehlerstand_280 { get; set; }
        public double? Zaehlerstand_Enfluri { get; set; }
        public DateTime? Uhrzeit { get; set; }
        public double? Temperatur_aussen { get; set; }
        public double? Temperatur_innen { get; set; }
        [MaxLength(250)]
        public string? Bemerkungen { get; set; }
    }
}
