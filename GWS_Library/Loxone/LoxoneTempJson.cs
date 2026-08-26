using Newtonsoft.Json;

namespace GWS_Library.Loxone
{
    public class LoxoneTempJson
    {
        [JsonProperty(PropertyName = "control")]
        public string Control { get; set; } = "";

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; } = "";

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; } = "";
    }
}
