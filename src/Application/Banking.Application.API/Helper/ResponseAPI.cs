using Newtonsoft.Json;
using System.Net;

namespace Banking.Application.API.Helper
{
    public class ResponseAPI<T> : ResponseAPI
    {
        public T Result { get; }

        public ResponseAPI(T result) : base(HttpStatusCode.OK, null)
        {
            Result = result;
        }
    }

    public class ResponseAPI
    {
        public HttpStatusCode StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ResponseAPI(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public static ResponseAPI Ok()
        {
            return new ResponseAPI(HttpStatusCode.OK);
        }

        public static ResponseAPI<T> Ok<T>(T result)
        {
            return new ResponseAPI<T>(result);
        }

        private static string GetDefaultMessageForStatusCode(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return "Success";

                case HttpStatusCode.BadRequest:
                    return "Something wrong!";

                case HttpStatusCode.NotFound:
                    return "Resource not found!";

                case HttpStatusCode.InternalServerError:
                    return "An unhandled error occurred!";

                default:
                    return null;
            }
        }
    }
}