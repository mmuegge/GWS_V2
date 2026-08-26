using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GWS_Library;
using Newtonsoft.Json;

namespace GWS_BlazorServer.Data
{
    public class WaterMeter
    {
        [JsonIgnore]
        public int ID_Tag { get; set; }

        [JsonProperty("ID_Anbieter")]
        public int Id_Supplier { get; set; }

        [JsonProperty("anbieter")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Datum eingeben!")]
        [DataType(DataType.Date)]
        [ValidDate(ErrorMessage ="Datum liegt in der Zukunft")]
        [JsonProperty("ablesetag")]
        public DateTime ReadingDate { get; set; }       // Ablesetag

        [Required(ErrorMessage = "Zählerstand eingeben!")]
        [JsonProperty("zaehlerstand")]
        public Double? MeterValueInside { get; set; }

        [Required(ErrorMessage = "Ablesezeit eingeben!")]
        [JsonProperty("uhrzeit")]
        public DateTime ReadingTime { get; set; }

        [JsonProperty("temperatur_aussen")]
        public Double? TemperatureOutside { get; set; }

        [JsonProperty("temperatur_innen")]
        public Double? TemperatureInside { get; set; }

        [JsonProperty("bemerkungen")]
        public string? Remarks { get; set; }
   
        [Required(ErrorMessage = "Zählerstand eingeben!")]
        [JsonProperty("zaehlerstand_aussen")]
        public Double? MeterValueOutside { get; set; }
    }
}
