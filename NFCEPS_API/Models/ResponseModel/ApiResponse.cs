namespace  NFCEPS_API.Wrapper;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
    public int StatusCode { get; set; } = 200;

    public static ApiResponse Ok(object? data = null, string message = "Success")
        => new()
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = 200
        };

    public static ApiResponse Fail(string message, int statusCode = 400)
        => new()
        {
            Success = false,
            Message = message,
            StatusCode = statusCode
        };
}