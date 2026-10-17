using Newtonsoft.Json;

namespace GWS_Statistics
{
    public class MyAppConfigModel
    {
        [JsonIgnore]
        public string MyAppSettingFile => "myAppSettings.json";

        public String GWSUri { get; set; } = string.Empty;
        public GWS_Supplier? Supplier_Id { get; set; }
        public GWS_Interval? Display_Interval { get; set; }
        public LoxoneModel? Loxone { get; set; }
    }

    public class GWS_Supplier
    {
        public string Gas { get; set; } = string.Empty;
        public string Water { get; set; } = string.Empty;
        public string Electric { get; set; } = string.Empty;
    }

    public class GWS_Interval
    {
        public string Gas { get; set; } = string.Empty;
        public string Water { get; set; } = string.Empty;
        public string Electric { get; set; } = string.Empty;
    }

    public class LoxoneModel
    {
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
