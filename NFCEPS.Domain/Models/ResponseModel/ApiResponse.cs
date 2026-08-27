using System.Text.Json.Serialization;

namespace NFCEPS.Domain.Models;

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

    public static ApiResponse FromDbResult<T>(T? result) where T : StatusResponse
    {
        if (result == null) return Fail("No response received from the database!", "-1");

        string defaultMsg = result.Status == "0" ? "Success" : "Failed";
        
        string resolvedMessage = ErrorCodes.GetMessage(result.Status) 
                                 ?? result.MSG 
                                 ?? defaultMsg;

        return new ApiResponse
        {
            Status = result.Status,
            Message = resolvedMessage,
            Data = result
        };
    }
}

