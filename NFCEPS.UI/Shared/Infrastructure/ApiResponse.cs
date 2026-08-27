using System.Text.Json.Serialization;

namespace NFCEPS.UI.Shared.Infrastructure;

// 1. Base Class: For actions that only return success/failure status (like SignUp)
public class ApiResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [JsonIgnore]
    public bool Success => Status == 0;

    public static ApiResponse Ok(string message = "Success", int status = 0)
        => new() { Status = status, Message = message };

    public static ApiResponse Fail(string message, int status = 1)
        => new() { Status = status, Message = message };
}

// 2. Generic Class: Inherits from base and adds a Data payload (like Login)
public class ApiResponse<T> : ApiResponse
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T? data = default, string message = "Success", int status = 0)
        => new() { Status = status, Message = message, Data = data };

    // Shadows the base Fail method to handle default generic data cleanly
    public static new ApiResponse<T> Fail(string message, int status = 1)
        => new() { Status = status, Message = message, Data = default };
}



