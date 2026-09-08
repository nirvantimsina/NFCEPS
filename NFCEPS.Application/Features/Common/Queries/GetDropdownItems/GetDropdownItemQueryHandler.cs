using System.Data;
using System.Linq;
using ErrorOr;
using MediatR;
using NFCEPS.Application.Common.Extensions;
using NFCEPS.Application.Features.Common.Queries.GetDropdownItems;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Common.Response; // Holds DbDropdownRow
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.Common.GetDropdownItems;

public class GetDropdownItemQueryHandler
    : IRequestHandler<GetDropdownItemsQuery, ErrorOr<List<DropdownListModel>>>
{
    private readonly IGenericRepository _repo;

    public GetDropdownItemQueryHandler(IGenericRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<List<DropdownListModel>>> Handle(
        GetDropdownItemsQuery request,
        CancellationToken token
    )
    {
        var dbParams = new { p_flag = request.Flag };

        var dbResult = await _repo.QueryAsync<DbDropdownRow>(
            "select * from get_all_dropdown(@p_flag)",
            dbParams,
            commandType: CommandType.Text
        );

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
