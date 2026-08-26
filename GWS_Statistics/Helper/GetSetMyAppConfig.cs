using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace GWS_Statistics.Helper
{
    public static class GetSetMyAppConfig
    {
            enum DisplayPeriod
            {
                daily =1,
                monthly,
                yearly
            }

        public static class GetMyAppSettings
        {
            /// <summary>
            /// Lesen der App-Settings
            /// </summary>
            /// <param name="myConfig"></param>
            /// <returns></returns>
            public static MyAppConfigModel? ReadSettings(MyAppConfigModel myConfig)
            {
                string appSettingsPath = "";
                MyAppConfigModel? config;

                try
                {
                    appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), myConfig.MyAppSettingFile);

                    var json = File.ReadAllText(appSettingsPath);
                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Converters.Add(new ExpandoObjectConverter());
                    jsonSettings.Converters.Add(new StringEnumConverter());

                    config = JsonConvert.DeserializeObject<MyAppConfigModel>(json);
                    return config;
                }
                catch (FileNotFoundException)
                {
                    config = SetMyAppSettings.CreateNewSettingFile(appSettingsPath);
                    return config;
                }

                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static class SetMyAppSettings
        {
            /// <summary>
            /// WriteSettings
            /// </summary>
            /// <param name="myConfig"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static bool WriteSettings(MyAppConfigModel myConfig, string key, string value)
            {
                try
                {
                    var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), myConfig.MyAppSettingFile);
                    if (File.Exists(appSettingsPath))
                    {
                        var json = File.ReadAllText(appSettingsPath);
                        var jsonSettings = new JsonSerializerSettings();
                        jsonSettings.Converters.Add(new ExpandoObjectConverter());
                        jsonSettings.Converters.Add(new StringEnumConverter());

                        dynamic? config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

                        if (config != null)
                        {
                            switch (key.ToLower())
                            {
                                case "supplier_id.gas":
                                    config.Supplier_Id.Gas = value ?? "0";
                                    break;

                                case "supplier_id.water":
                                    config.Supplier_Id.Water = value ?? "0";
                                    break;

                                case "supplier_id.electric":
                                    config.Supplier_Id.Electric = value ?? "0";
                                    break;

                                case "display_interval.gas":
                                    config.Display_Interval.Gas = value ?? nameof(DisplayPeriod.daily);
                                    break;

                                case "display_interval.water":
                                    config.Display_Interval.Water = value ?? nameof(DisplayPeriod.daily);
                                    break;

                                case "display_interval.electric":
                                    config.Display_Interval.Electric = value ?? nameof(DisplayPeriod.daily);
                                    break;
                            }

                            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

                            File.WriteAllText(appSettingsPath, newJson);

                            return true;
                        }
                        else
                        { 
                            return false; 
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// Erzeugen einer neuen Datei "MyAppSettings"
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static MyAppConfigModel CreateNewSettingFile(string fileName)
            {
                MyAppConfigModel config = new()
                {
                    GWSUri = "http://192.168.178.156:5850/api/",
                    Supplier_Id = new GWS_Supplier { Gas = "0", Electric = "0", Water = "0" },
                    Display_Interval = new GWS_Interval {  Gas = nameof(DisplayPeriod.daily), Electric= nameof(DisplayPeriod.daily), Water= nameof(DisplayPeriod.daily) },
                };

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new ExpandoObjectConverter());
                jsonSettings.Converters.Add(new StringEnumConverter());
                var jsonString = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);
                File.WriteAllText(fileName, jsonString);

                return config;
            }
        }
    }
}
