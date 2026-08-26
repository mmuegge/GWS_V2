using GWS_Library;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GWS_BlazorServer.Data
{
    public class ElectricMeter
    {
        [JsonIgnore]
        //[Bindable(false)]
        public int ID_Tag { get; set; }

        [JsonProperty("ID_Anbieter")]
        //[Bindable(false)]
        public int Id_Supplier { get; set; }

        [JsonProperty("anbieter")]
        [Display(Name = "Anbieter")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Datum eingeben!")]
        [DataType(DataType.Date)]
        [ValidDate(ErrorMessage = "Datum liegt in der Zukunft")]
        [JsonProperty("ablesetag")]
        [Display(Name = "Ablesedatum")]
        public DateTime ReadingDate { get; set; }

        [Required(ErrorMessage = "Ablesezeit eingeben!")]
        [JsonProperty("uhrzeit")]
        [Display(Name = "Ablesezeit")]
        public DateTime ReadingTime { get; set; }

        [Required(ErrorMessage = "Zählerstand eingeben!")]
        [JsonProperty("zaehlerstand")]
        [Display(Name = "Netzstrom-Bezug (1.8.0)")]
        public Double? MeterValue_In { get; set; }

        [Required(ErrorMessage = "Zählerstand eingeben!")]
        [JsonProperty("zaehlerstand_280")]
        [Display(Name = "Netzstrom-Einspeisung (2.8.0)")]
        public Double? MeterValue_Out { get; set; }

        [Required(ErrorMessage = "Zählerstand eingeben!")]
        [JsonProperty("zaehlerstand_Enfluri")]
        [Display(Name = "Enfluri")]
        public Double? MeterValue_Enfluri { get; set; }

        [JsonProperty("temperatur_innen")]
        [Display(Name = "Innentemperatur")]
        public Double? TemperatureInside { get; set; }

        [JsonProperty("temperatur_aussen")]
        [Display(Name = "Aussentemperatur")]
        public Double? TemperatureOutside { get; set; }

        [JsonProperty("bemerkungen")]
        [Display(Name = "Bemerkungen")]
        public string? Remarks { get; set; }
    }
}
