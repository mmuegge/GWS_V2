using Newtonsoft.Json;

namespace GWS_Statistics.Data
{
    public class GWS_PaymentModel: IPayments
    {
        [JsonProperty("iD_Zahlung")]
        public int ID_Zahlung { get; set; }

        [JsonProperty("iD_Anbieter")]
        public int ID_Anbieter { get; set; }

        [JsonProperty("anbieter")]
        public string? Anbieter { get; set; }

        [JsonProperty("datum")]
        public DateTime Datum { get; set; }

        [JsonProperty("zahlungsart")]
        public string? Zahlungsart { get; set; }

        [JsonProperty("zahlungen")]
        public double? Zahlungen { get; set; }

        [JsonProperty("bemerkungen")]
        public string? Bemerkungen { get; set; }
    }
}
