using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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

        #region class GetMyAppSettings
        /// <summary>
        /// 
        /// </summary>
        public static class GetMyAppSettings
        {
            /// <summary>
            ///  Lesen der Einstellungen aus der Config-Datei
            /// </summary>
            /// <param name="myConfig"></param>
            /// <returns></returns>
            public static async Task<MyAppConfigModel?> ReadSettingsAsync(MyAppConfigModel myConfig)
            {
                // Fallback auf den Standardnamen
                string fileName = myConfig?.MyAppSettingFile ?? "myAppSettings.json";
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                if (!File.Exists(appSettingsPath))
                {
                    // await direkt weitergeben (spart Zustandsautomat, falls CreateNewSettingFileAsync async ist)
                    return await SetMyAppSettings.CreateNewSettingFileAsync(appSettingsPath);
                }

                try
                {
                    // Optimierter FileStream: Echtes asynchrones I/O und erlaubt paralleles Lesen (FileShare.Read)
                    using FileStream openStream = new(
                        appSettingsPath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        bufferSize: 4096,
                        useAsync: true);

                    return await JsonSerializer.DeserializeAsync<MyAppConfigModel>(openStream, JsonOptions);
                }
                catch (JsonException)
                {
                    // Ungültiges JSON abfangen und null zurückgeben
                    return null;
                }
                // Allgemeiner Catch-Block entfernt: Fehler fliegen direkt hoch, wodurch der originale Stacktrace perfekt erhalten bleibt.
            }
        }
        #endregion

        #region class SetMyAppSettings
        public static class SetMyAppSettings
        {
            #region WriteSettingsAsync
            /// <summary> 
            /// Schreiben der Einstellungen
            /// </summary>
            /// <param name="myConfig"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static async Task<bool> WriteSettingsAsync(MyAppConfigModel myConfig, string key, string value)
            {
                if (string.IsNullOrWhiteSpace(key)) return false;

                // 1. Key im Vorfeld validieren und splitten (Zerschneidet z. B. "supplier_id.gas" in "Supplier_Id" und "Gas")
                int dotIndex = key.IndexOf('.');
                if (dotIndex <= 0 || dotIndex == key.Length - 1) return false;

                // Memory-effizientes Extrahieren der JSON-Knotennamen (Erfordert .NET 8+)
                string mainKey = string.Create(key.Length, key, (span, state) =>
                {
                    state.AsSpan().CopyTo(span);
                    // Erste Sektion PascalCase annähern (oder exakt matchen)
                    span[0] = char.ToUpperInvariant(span[0]);
                }).Substring(0, dotIndex);

                // Einfachere, lesbare Variante für die Key-Aufbereitung:
                string[] parts = key.Split('.');
                if (parts.Length != 2) return false;

                // Standardwerte setzen
                string finalValue = value ?? (parts[0].Equals("display_interval", StringComparison.OrdinalIgnoreCase)
                    ? nameof(DisplayPeriod.daily)
                    : "0");

                string fileName = myConfig?.MyAppSettingFile ?? "myAppSettings.json";
                string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                if (!File.Exists(appSettingsPath)) return false;

                // Temporärer Pfad für atomares Schreiben (Schützt vor Datenverlust bei Absturz)
                string tempPath = appSettingsPath + ".tmp";

                try
                {
                    JsonNode? rootNode;

                    // 2. Speicher- und CPU-effizientes Einlesen direkt aus dem Stream
                    using (FileStream readStream = new FileStream(appSettingsPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                    {
                        rootNode = await JsonNode.ParseAsync(readStream);
                    }

                    if (rootNode is not JsonObject jsonObject) return false;

                    // 3. Dynamische Zuordnung statt riesigem Switch-Case
                    // Findet "Supplier_Id" oder "Display_Interval" unabhängig von Groß-/Kleinschreibung im JSON
                    string actualMainKey = "Supplier_Id";
                    string actualSubKey = "Gas";

                    if (parts[0].Equals("display_interval", StringComparison.OrdinalIgnoreCase)) actualMainKey = "Display_Interval";
                    if (parts[1].Equals("water", StringComparison.OrdinalIgnoreCase)) actualSubKey = "Water";
                    else if (parts[1].Equals("electric", StringComparison.OrdinalIgnoreCase)) actualSubKey = "Electric";

                    // Unterknoten sicher anlegen, falls nicht vorhanden
                    if (jsonObject[actualMainKey] == null)
                    {
                        jsonObject[actualMainKey] = new JsonObject();
                    }

                    // Wert setzen
                    jsonObject[actualMainKey]![actualSubKey] = finalValue;

                    // 4. Atomares Schreiben: Erst in Temp-Datei schreiben, dann ersetzen
                    using (FileStream writeStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                    {
                        await JsonSerializer.SerializeAsync(writeStream, rootNode, JsonOptions);
                    }

                    // Datei sicher ersetzen (Ausfallsicher!)
                    File.Move(tempPath, appSettingsPath, overwrite: true);
                    return true;
                }
                catch (Exception)
                {
                    // Temp-Datei aufräumen, falls vorhanden
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                    return false;
                }
            }
            #endregion

            #region CreateNewSettingFileAsync
            /// <summary>
            /// Schreiben einer neuen Einstellungsdatei
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static async Task<MyAppConfigModel> CreateNewSettingFileAsync(string filePath)
            {
                //MyAppConfigModel config = new()
                //{
                //    GWSUri = "http://192.168.178",
                //    Supplier_Id = new GWS_Supplier { Gas = "0", Electric = "0", Water = "0" },
                //    Display_Interval = new GWS_Interval { Gas = nameof(DisplayPeriod.daily), Electric = nameof(DisplayPeriod.daily), Water = nameof(DisplayPeriod.daily) },
                //};

                //await using FileStream createStream = File.Create(fileName);
                //await JsonSerializer.SerializeAsync(createStream, config, JsonOptions);

                //return config;

                // C# 9+ Target-Typed New: Kompakt und allokationsarm initialisiert
                MyAppConfigModel config = new()
                {
                    GWSUri = "http://192.168.178",
                    Supplier_Id = new GWS_Supplier { Gas = "0", Electric = "0", Water = "0" },
                    Display_Interval = new GWS_Interval
                    {
                        Gas = nameof(DisplayPeriod.daily),
                        Electric = nameof(DisplayPeriod.daily),
                        Water = nameof(DisplayPeriod.daily)
                    },
                };

                // Temporärer Pfad für atomares Erstellen (Schutz vor Teil-Schreibfehlern)
                string tempPath = filePath + ".tmp";

                try
                {
                    // 1. Expliziter FileStream mit echtem asynchronem I/O (useAsync: true)
                    using (FileStream createStream = new(
                        tempPath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        useAsync: true))
                    {
                        await JsonSerializer.SerializeAsync(createStream, config, JsonOptions);
                    }

                    // 2. Atomares Ersetzen/Verschieben: Verhindert korrupte (leere) Dateien bei Systemabsturz
                    File.Move(tempPath, filePath, overwrite: true);
                }
                catch (Exception)
                {
                    // Temp-Datei bei Fehler aufräumen
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                    throw; // Fehler nach oben weiterreichen, da die App wissen muss, dass die Konfig nicht erstellt werden konnte
                }

                return config;
            }
        }
            #endregion
    }
        #endregion
}