using ErrorOr;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Common.Extensions;

public static class DatabaseResultExtensions
{
    public static ErrorOr<T> ToDbResult<T>(this T? result) where T : class
    {
        if (result == null)
        {
            return Error.NotFound(description: "No record received from the server!");
        }

        dynamic dynamicResult = result;

        if (dynamicResult.Status != "0")
        {
            return Error.Validation(code: dynamicResult.Status, description: dynamicResult.MSG);
        }

        return result;
    }

    public static ErrorOr<List<T>> ToDbResultList<T>(this IEnumerable<T>? result) where T : StatusResponse
    {
        if (result == null)
        {
            return Error.NotFound(description: "No record received from the server!");
        }

        var list = result.ToList();
        var firstItem = list.FirstOrDefault();

        // If the database returned a row containing a status error
        if (firstItem != null && firstItem.Status != "0" && firstItem.Status != null)
        {
            return Error.Validation(
                code: firstItem.Status,
                description: firstItem.MSG ?? "An unexpected database validation error occurred."
            );
        }

        return list;
    }

}
