namespace NFCEPS.Domain.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }

    public static ApiResponse Ok(object? data = null,
        string message = "Success")
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse Fail(string message)
        => new() { Success = false, Message = message };

    public static ApiResponse FromDbResult<T>(T? result) where T : StatusResponse
    {
        if (result == null) return Fail("No response received from the database!");

        return result.Status == 0 
            ? Ok(result, result.MSG ?? "Success") 
            : Fail(result.MSG ?? "Failed");
    }
}

