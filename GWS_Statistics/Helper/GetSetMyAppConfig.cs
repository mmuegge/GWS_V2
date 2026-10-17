using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using GWS_Statistics;

namespace GWS_Statistics.Helper
    {
        public static class GetSetMyAppConfig
        {
            // Lokales Enum für die Standardwerte im Code belassen
            private enum DisplayPeriod
            {
                daily = 1,
                monthly,
                yearly
            }

            // Gemeinsame, statische Optionen sparen CPU-Zyklen und RAM bei jedem Aufruf
            private static readonly JsonSerializerOptions JsonOptions = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            public static class GetMyAppSettings
            {
                public static async Task<MyAppConfigModel?> ReadSettingsAsync(MyAppConfigModel myConfig)
                {
                    // Fallback auf den Standardnamen, falls myConfig oder die Eigenschaft null ist
                    string fileName = myConfig?.MyAppSettingFile ?? "myAppSettings.json";

                    try
                    {
                        string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                        if (!File.Exists(appSettingsPath))
                        {
                            return await SetMyAppSettings.CreateNewSettingFileAsync(appSettingsPath);
                        }

                        using FileStream openStream = File.OpenRead(appSettingsPath);
                        return await JsonSerializer.DeserializeAsync<MyAppConfigModel>(openStream, JsonOptions);
                    }
                    catch (JsonException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            public static class SetMyAppSettings
            {
                public static async Task<bool> WriteSettingsAsync(MyAppConfigModel myConfig, string key, string value)
                {
                    string fileName = myConfig?.MyAppSettingFile ?? "myAppSettings.json";
                    if (string.IsNullOrWhiteSpace(key)) return false;

                    try
                    {
                        string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        if (!File.Exists(appSettingsPath)) return false;

                        string json = await File.ReadAllTextAsync(appSettingsPath);
                        JsonNode? rootNode = JsonNode.Parse(json);
                        if (rootNode == null) return false;

                        string safeValue = value ?? "0";
                        string safeInterval = value ?? nameof(DisplayPeriod.daily);

                        // Korrekte und sichere Erstellung von Unterknoten für das JsonNode-Framework
                        switch (key.ToLowerInvariant())
                        {
                            case "supplier_id.gas":
                                if (rootNode["Supplier_Id"] == null) rootNode["Supplier_Id"] = new JsonObject();
                                rootNode["Supplier_Id"]!["Gas"] = safeValue;
                                break;

                            case "supplier_id.water":
                                if (rootNode["Supplier_Id"] == null) rootNode["Supplier_Id"] = new JsonObject();
                                rootNode["Supplier_Id"]!["Water"] = safeValue;
                                break;

                            case "supplier_id.electric":
                                if (rootNode["Supplier_Id"] == null) rootNode["Supplier_Id"] = new JsonObject();
                                rootNode["Supplier_Id"]!["Electric"] = safeValue;
                                break;

                            case "display_interval.gas":
                                if (rootNode["Display_Interval"] == null) rootNode["Display_Interval"] = new JsonObject();
                                rootNode["Display_Interval"]!["Gas"] = safeInterval;
                                break;

                            case "display_interval.water":
                                if (rootNode["Display_Interval"] == null) rootNode["Display_Interval"] = new JsonObject();
                                rootNode["Display_Interval"]!["Water"] = safeInterval;
                                break;

                            case "display_interval.electric":
                                if (rootNode["Display_Interval"] == null) rootNode["Display_Interval"] = new JsonObject();
                                rootNode["Display_Interval"]!["Electric"] = safeInterval;
                                break;

                            default:
                                return false;
                        }

                        await using FileStream writeStream = File.Create(appSettingsPath);
                        await JsonSerializer.SerializeAsync(writeStream, rootNode, JsonOptions);

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                public static async Task<MyAppConfigModel> CreateNewSettingFileAsync(string fileName)
                {
                    MyAppConfigModel config = new()
                    {
                        GWSUri = "http://192.168.178",
                        Supplier_Id = new GWS_Supplier { Gas = "0", Electric = "0", Water = "0" },
                        Display_Interval = new GWS_Interval { Gas = nameof(DisplayPeriod.daily), Electric = nameof(DisplayPeriod.daily), Water = nameof(DisplayPeriod.daily) },
                    };

                    await using FileStream createStream = File.Create(fileName);
                    await JsonSerializer.SerializeAsync(createStream, config, JsonOptions);

                    return config;
                }
            }
        }
    }


