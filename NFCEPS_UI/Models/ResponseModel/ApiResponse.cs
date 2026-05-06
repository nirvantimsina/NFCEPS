namespace NFCEPS_UI.Models.ResponseModel;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T? data = default, string message = "Success")
        => new()
        {
            Success = true,
            Message = message,
            Data = data
        };

    public static ApiResponse<T> Fail(string message)
        => new()
        {
            Success = false,
            Message = message,
            Data = default
        };
}