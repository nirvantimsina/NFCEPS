using System.Text.Json.Serialization;

namespace NFCEPS.Domain.Models;

public class ApiResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [JsonPropertyName("data")]
    public object? Data { get; set; }

    [JsonIgnore]
    public bool Success => Status == 0;

    public static ApiResponse Ok(object? data = null, string message = "Success", int status = 0)
        => new() { Status = status, Message = message, Data = data };

    public static ApiResponse Fail(string message, int status = 1)
        => new() { Status = status, Message = message };

    public static ApiResponse FromDbResult<T>(T? result) where T : StatusResponse
    {
        if (result == null) return Fail("No response received from the database!", -1);

        return new ApiResponse
        {
            Status = result.Status,
            Message = result.MSG ?? (result.Status == 0 ? "Success" : "Failed"),
            Data = result
        };
    }
}

