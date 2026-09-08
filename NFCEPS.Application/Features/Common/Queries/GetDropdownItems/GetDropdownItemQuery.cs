using ErrorOr;
using MediatR;
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.Common.Queries.GetDropdownItems;

public class GetDropdownItemsQuery : IRequest<ErrorOr<List<DropdownListModel>>>
{
    public string? Flag { get; set; }
}
