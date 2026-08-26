using Newtonsoft.Json;

namespace GWS_Library
{
    public class MyAppConfig
    {
        [JsonIgnore]
        private const string _myAppConfigFile = "myAppSettings.json";

        public string MyAppSettingFile
        {
            get { return _myAppConfigFile; }
        }

        public String GWSUri { get; set; } = string.Empty;
        public GWS? SupplierId;

        public LoxoneData? Loxone { get; set; }
    }

    public class GWS
    {
        public string? Gas { get; set; }
        public string? Water { get; set; }
        public string? Electric { get; set; }
    }

    public class LoxoneData
    {
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
