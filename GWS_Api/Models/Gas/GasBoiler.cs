using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWS_Api.Models.Gas
{
    public class GasBoiler
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Verbrauchsjahr { get; set; }
        public double? Gesamt_Verbrauch { get; set; }
        public double? Heizung_Verbrauch { get; set; }
        public double? Warmwasser_Verbrauch { get; set; }
        public double? Strom_Verbrauch { get; set; }
        [MaxLength(250)]
        public string? Bemerkung { get; set; }
    }
}
