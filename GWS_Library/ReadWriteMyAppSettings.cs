using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace GWS_Library
{
    public static class ReadWriteMyAppSettings
    {
        /// <summary>
        /// Lesen der App-Settings
        /// </summary>
        /// <param name="myConfig"></param>
        /// <returns></returns>
        public static MyAppConfig? ReadMyAppSettings(MyAppConfig myConfig)
        {
            string appSettingsPath = "";
            try
            {
                appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), myConfig.MyAppSettingFile);

                var json = File.ReadAllText(appSettingsPath);
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new ExpandoObjectConverter());
                jsonSettings.Converters.Add(new StringEnumConverter());

                MyAppConfig? config = JsonConvert.DeserializeObject<MyAppConfig>(json);
                return config;

            }
            catch (FileNotFoundException)
            {
                MyAppConfig config = CreateNewAppSettingFile(appSettingsPath);
                return config;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Speichern der App-Settings
        /// </summary>
        /// <param name="myConfig"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WriteMyAppSettings(MyAppConfig myConfig, string key, string value)
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
                            case "gas":
                                config.SupplierId.Gas = value ?? "0";
                                break;

                            case "water":
                                config.SupplierId.Water = value ?? "0";
                                break;

                            case "electric":
                                config.SupplierId.Electric = value ?? "0";
                                break;
                        }
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
        public static MyAppConfig CreateNewAppSettingFile(string fileName)
        {
            MyAppConfig config = new MyAppConfig
            {
                GWSUri = "http://192.168.178.52:5600/api/",
                SupplierId = new GWS { Gas = "0", Electric = "0", Water = "0" },
                //Loxone = new LoxoneData { User = "muegge", Password = "loxone_MMwiK27243Ew18" }
                Loxone = new LoxoneData { User = "", Password = "" }
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
