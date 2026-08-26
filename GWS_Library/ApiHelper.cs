using System.Net.Http.Headers;

namespace GWS_Library
{
    public class ApiHelper
    {
        public static HttpClient? GWSApiClient { get; set; }

        public static HttpClient? LoxoneApiClient { get; set; }

        public static void InitializeClient()
        {
            GWSApiClient = new();
            GWSApiClient.DefaultRequestHeaders.Accept.Clear();
            GWSApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));    // der API wird mitgeteilt Rückgabe soll Json sein
            GWSApiClient.Timeout = new TimeSpan(0, 0, 2); ;
        }

        #region Ausschneiden eines Teil-Strings ab/bis einer bestimmten Position in einem String
        public static string GetStringBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
