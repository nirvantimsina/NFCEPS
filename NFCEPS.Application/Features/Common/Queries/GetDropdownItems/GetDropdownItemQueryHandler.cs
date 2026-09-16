using System.Data;
using ErrorOr;
using MediatR;
using NFCEPS.Application.Common.Extensions;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Common.Response; // Holds DbDropdownRow
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.Common.Queries.GetDropdownItems;

public class GetDropdownItemQueryHandler
    : IRequestHandler<GetDropdownItemQuery, ErrorOr<List<DropdownListModel>>>
{
    private readonly IGenericRepository _repo;

    public GetDropdownItemQueryHandler(IGenericRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<List<DropdownListModel>>> Handle(
        GetDropdownItemQuery request,
        CancellationToken token
    )
    {
        var dbParams = new { p_flag = request.Flag };

        var dbResult = await _repo.QueryAsync<DbDropdownRow>(
            "select * from get_dropdown_data(@p_flag)",
            dbParams,
            commandType: CommandType.Text
        );

        Console.WriteLine($"DB raw count retrieved: {dbResult?.Count() ?? 0}");
        if (dbResult?.Any() == true)
        {
            Console.WriteLine(
                $"First Item Text: {dbResult.First().Text}, Value: {dbResult.First().Value}"
            );
        }
        ErrorOr<List<DbDropdownRow>> validationResult = dbResult.ToDbResultList();

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        return validationResult
            .Value.Select(r => new DropdownListModel { Text = r.Text, Value = r.Value })
            .ToList();
    }
}
