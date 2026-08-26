using System.Net;

namespace GWS_Library
{
    public class ApiResponse
    { 
        public ApiResponse(HttpStatusCode httpStatus, String response) 
        {
            MyHttpStatusCode = httpStatus;
            MyResponseString= response;
        }

        public HttpStatusCode? MyHttpStatusCode { get; set; }
        public string? MyResponseString { get; set; }
    }
}
