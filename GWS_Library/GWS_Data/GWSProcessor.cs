using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace GWS_Library.GWS_Data
{
    public class GWSProcessor
    {
        HttpClient _gwsClient;
        readonly string? baseUrl = string.Empty;
        public GWSProcessor(HttpClient gwsClient, string url)
        {
            baseUrl = url;
            _gwsClient = gwsClient;
            _gwsClient.DefaultRequestHeaders.Accept.Clear();
            _gwsClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;charset=utf-8");    // der API wird mitgeteilt Rückgabe soll Json sein
        }

        #region READ
        public async Task<T> LoadReadingDateAsync<T>(string url, DateTime? readingDate)
        {
            string requestUrl;
            T? dataData = default;

            if (readingDate != null)
            {
                requestUrl = baseUrl + url + readingDate.GetValueOrDefault().Year.ToString() + '.' + readingDate.GetValueOrDefault().Month.ToString() + '.' + readingDate.GetValueOrDefault().Day.ToString();
            }
            else
            {
                requestUrl = baseUrl + $"/info.0.json";
            }

            using HttpResponseMessage response = await _gwsClient.GetAsync(requestUrl);
            {
                HttpStatusCode statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    T dateData = await response.Content.ReadAsAsync<T>();   // liest Daten im Json-Format und versucht es in gasTarif zu konvertieren

                    return dateData;
                }

                return dataData!;
            }
        }

        public async Task<List<T>?> LoadDataAsync<T>(string url)
        {
            List<T>? dataList = [];
            string requestUrl = baseUrl + url;
            using HttpResponseMessage response = await _gwsClient.GetAsync(requestUrl);
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dataList = JsonConvert.DeserializeObject<List<T>>(responseBody);
                }
                return dataList;
            }
        }

        //public async Task<List<T>?> LoadDataByIdAsync<T>(string url, int id)
        //{
        //    List<T>? dataList = [];

        //    string requestUrl = baseUrl + url + "/" + id.ToString();
        //    using HttpResponseMessage response = await _gwsClient.GetAsync(requestUrl);
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            dataList = JsonConvert.DeserializeObject<List<T>>(responseBody);
        //        }
        //        return dataList;
        //    }
        //}
        #endregion

        public async Task<HttpStatusCode> CheckReadingDateAvailableAsync(string url, DateTime readingDate)
        {
            string requestUrl;

            try
            {
                requestUrl = baseUrl + url + readingDate.Year.ToString() + '.' + readingDate.Month.ToString() + '.' + readingDate.Day.ToString();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            using HttpResponseMessage response = await _gwsClient.GetAsync(requestUrl);
            {
                return response.StatusCode;
            }
        }

        #region POST
        public async Task<ApiResponse> PostDataAsync<T>(string url, T data)
        {
            ApiResponse apiResponse = new(HttpStatusCode.BadRequest, String.Empty); // Response-Objekt erzeugen über Konstruktor

            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd'T'HH:mm:ss"
            };
            var json = JsonConvert.SerializeObject(data, jsonSettings);

            var content = new StringContent(json, encoding: Encoding.UTF8, mediaType: "application/json");
            using HttpResponseMessage response = await _gwsClient.PostAsync(baseUrl + url, content);
            {
                apiResponse.MyResponseString = await response.Content.ReadAsStringAsync();  // Rückgabewert an Aufrufer zurückgeben
                apiResponse.MyHttpStatusCode = response.StatusCode;
            }
            return apiResponse;
        }
        #endregion

        #region DELETE
        public async Task<T> DeleteDataAsync<T>(string url, string dataToDelete)
        {
            string requestUrl = baseUrl + url + dataToDelete;

            using HttpResponseMessage response = await _gwsClient.DeleteAsync(requestUrl);
            {
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();

                    return data;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        #endregion

        #region UPDATE
        public async Task<ApiResponse> UpdateDataAsync<T>(string url, T data)
        {
            ApiResponse apiResponse = new ApiResponse(HttpStatusCode.BadRequest, String.Empty); // Response-Objekt erzeugen über Konstruktor

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss";
            var json = JsonConvert.SerializeObject(data, jsonSettings);

            var content = new StringContent(json, encoding: Encoding.UTF8, mediaType: "application/json");

            using (HttpResponseMessage response = await _gwsClient.PutAsync(baseUrl + url, content))
            {
                apiResponse.MyResponseString = await response.Content.ReadAsStringAsync();  // Rückgabewert an Aufrufer zurückgeben
                apiResponse.MyHttpStatusCode = response.StatusCode;
            }
            return apiResponse;
        }
        #endregion
    }
}
