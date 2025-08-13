using System.Text.Json.Serialization;

namespace SharedKernel.Models
{
    public class ApiResponse<T>
    {

        [JsonPropertyName("meta_data")]
        public MetaData MetaData { get; set; } = new MetaData();

        [JsonPropertyName("data")]
        public T Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, int statusCode = 200, string message = "Success")
        {
            Data = data;
            MetaData.StatusCode = statusCode;
            MetaData.Message = message;
            MetaData.Timestamp = DateTime.UtcNow;
        }
    }

    public class MetaData
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("time_stamp")]
        public DateTime Timestamp { get; set; }
    }
}
