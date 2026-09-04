using NFCEPS.Application.Behavior;
using NFCEPS.Domain.Models;
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
                logger.LogError(ex, "Unhandeled Exception - {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = ex switch
            {
                CustomValidationException valEx => (
                    (int)HttpStatusCode.BadRequest,
                    string.Join(" | ", valEx.Errors.Select(e =>
                        $"{e.OccuredIn}: {ErrorCodes.GetMessage(e.ErrorCode ?? "1")}"))
                ),

                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, ex.Message),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, ex.Message),
                ArgumentException => ((int)HttpStatusCode.BadRequest, ex.Message),

                DbException dbEx when dbEx.SqlState == "23505" =>
                    ((int)HttpStatusCode.Conflict, "This record already exists!"),
                DbException dbEx when dbEx.SqlState == "23502" =>
                    ((int)HttpStatusCode.BadRequest, "A required field is missing!"),
                DbException dbEx when dbEx.SqlState == "23503" =>
                    ((int)HttpStatusCode.BadRequest, "This record is associated with other data and cannot be modified!"),

                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occured!")
            };

            context.Response.StatusCode = statusCode;
            var response = ApiResponse.Fail(message);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
