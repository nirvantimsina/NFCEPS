using System.Text.Json.Serialization;

namespace NFCEPS.Shared.Wrappers
{
    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("msg")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonIgnore]
        public bool Success => Status == "0";

        public static ApiResponse Ok(object? data = null, string message = "Success", string status = "0")
            => new() { Status = status, Message = message, Data = data };

        public static ApiResponse Fail(string message, string status = "1")
            => new() { Status = status, Message = message };
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonPropertyName("data")]
        public new T? Data 
        { 
            get => base.Data is T typedData ? typedData : default; 
            set => base.Data = value; 
        }

        public static ApiResponse<T> Ok(T? data = default, string message = "Success", string status = "0")
            => new() { Status = status, Message = message, Data = data };

        public static new ApiResponse<T> Fail(string message, string status = "1")
            => new() { Status = status, Message = message };
    }
}
