using Newtonsoft.Json;

namespace GWS_Library.Loxone
{
    public class LoxoneModel
    {
        [JsonProperty(PropertyName = "LL")]
        public LoxoneTempJson? LL { get; set; }
    }
}
