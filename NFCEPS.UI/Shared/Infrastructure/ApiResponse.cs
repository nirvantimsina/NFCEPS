namespace NFCEPS.UI.Shared.Infrastructure;

// 1. Base Class: For actions that only return success/failure status (like SignUp)
public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static ApiResponse Ok(string message = "Success")
        => new() { Success = true, Message = message };

    public static ApiResponse Fail(string message)
        => new() { Success = false, Message = message };
}

// 2. Generic Class: Inherits from base and adds a Data payload (like Login)
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T? data = default, string message = "Success")
        => new() { Success = true, Message = message, Data = data };

    // Shadows the base Fail method to handle default generic data cleanly
    public static new ApiResponse<T> Fail(string message)
        => new() { Success = false, Message = message, Data = default };
}



