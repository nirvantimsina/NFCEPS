using NFCEPS.Shared.Wrappers;
using System.Data.Common;
using System.Net;
using System.Text.Json;

namespace NFCEPS.Presentation.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled Exception - {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            (int statusCode, string errorCode, string message) = ex switch
            {
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, ErrorCodes.Unauthorized!, ex.Message),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, ErrorCodes.RecordNotFound!, ex.Message),
                ArgumentException => ((int)HttpStatusCode.BadRequest, ErrorCodes.GeneralError!, ex.Message),

                DbException dbEx when dbEx.SqlState == "23505" =>
                    ((int)HttpStatusCode.Conflict, ErrorCodes.UserAlreadyExists!, "This record already exists!"),
                DbException dbEx when dbEx.SqlState == "23502" =>
                    ((int)HttpStatusCode.BadRequest, ErrorCodes.MissingRequiredField!, "A required field is missing!"),
                DbException dbEx when dbEx.SqlState == "23503" =>
                    ((int)HttpStatusCode.BadRequest, ErrorCodes.GeneralError!, "This record is associated with other data and cannot be modified!"),

                _ => ((int)HttpStatusCode.InternalServerError, ErrorCodes.GeneralError!, "An unexpected error occurred!")
            };

            context.Response.StatusCode = statusCode;

            var response = ApiResponse.Fail(message, errorCode);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
