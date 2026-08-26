using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace GWS_Library.Loxone
{
    public class LoxoneProcessor
    {
        private static LoxoneTemperatures temperatures = new LoxoneTemperatures();              // Temperaturen

        // Abfrage XML-Format
        //private const string endpointAussenTemp = "http://192.168.178.23/dev/sps/io/Aussentemperatur/state";  //(dev --> XML)
        //private const string endpointAussenTemp = "http://192.168.178.23/dev/sps/io/Aussentemperatur/state";  //(dev --> XML)

        // Abfrage Json-Format
        private const string endpointAussenTemp = "http://192.168.178.23/jdev/sps/io/Aussentemperatur/state";   //(jdev --> Json)   
        private const string endpointInnenTemp = "http://192.168.178.23/jdev/sps/io/WZ-Temperatur/state";       //(jdev --> Json)
        private static CredentialCache? myCredentialCache = null;

        public static async Task<LoxoneTemperatures> LoadLoxoneTemp(string user, string password)
        {
            CultureInfo culture;
            NumberStyles style;
            culture = CultureInfo.CreateSpecificCulture("en-GB");
            style = NumberStyles.Number;
            myCredentialCache = [];

            try
            {
                //myCredentialCache.Add(new Uri(endpointAussenTemp), "Basic", new NetworkCredential(config.Loxone.User, config.Loxone.Password));
                myCredentialCache.Add(new Uri(endpointAussenTemp), "Basic", new NetworkCredential(user, password));
                using var handler = new HttpClientHandler { Credentials = myCredentialCache, PreAuthenticate = true };
                {
                    ApiHelper.LoxoneApiClient = new(handler);
                    var url = endpointAussenTemp;
                    double number;
                    string strTemp;

                    using (Stream responseStream = await ApiHelper.LoxoneApiClient.GetStreamAsync(url))
                    {
                        if (responseStream != null)
                        {
                            using StreamReader reader = new(responseStream);
                            {
                                try
                                {
                                    string strResponseValue = reader.ReadToEnd();
                                    LoxoneModel? loxoneTemperature = JsonConvert.DeserializeObject<LoxoneModel>(strResponseValue);
                                    if (loxoneTemperature != null)
                                    {
                                        if (loxoneTemperature.LL!.Code == "200")
                                        {
                                            strTemp = loxoneTemperature.LL.Value.Replace("°", "");
                                            bool success = double.TryParse(strTemp, style, culture, out number);
                                            temperatures.Aussentemperatur = success == true ? number : 0.0;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    temperatures.Aussentemperatur = 99.0;
                                }
                            } // end of Streamreader
                        }
                    }

                    // myCredentialCache.Add(new Uri(endpointInnenTemp), "Basic", new NetworkCredential(config.Loxone.User, config.Loxone.Password));
                    myCredentialCache.Add(new Uri(endpointInnenTemp), "Basic", new NetworkCredential(user, password));
                    url = endpointInnenTemp;
                    using (Stream responseStream = await ApiHelper.LoxoneApiClient.GetStreamAsync(url))
                    {
                        if (responseStream != null)
                        {
                            using StreamReader reader = new(responseStream);
                            {
                                try
                                {
                                    string strResponseValue = reader.ReadToEnd();
                                    LoxoneModel? loxoneTemperature = JsonConvert.DeserializeObject<LoxoneModel>(strResponseValue);
                                    if (loxoneTemperature != null)
                                    {
                                        if (loxoneTemperature.LL!.Code == "200")
                                        {
                                            strTemp = loxoneTemperature.LL.Value.Replace("°", "");
                                            bool success = double.TryParse(strTemp, style, culture, out number);
                                            temperatures.Innentemperatur = success == true ? number : 0.0;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    temperatures.Innentemperatur = 99.0;
                                }
                            } // end of Streamreader
                        }
                    }
                    return temperatures;
                }
            }
            catch (Exception)
            {
                temperatures.Aussentemperatur = 99.0;
                temperatures.Innentemperatur = 99.0;
                return temperatures;
            }
        }

    }
}
