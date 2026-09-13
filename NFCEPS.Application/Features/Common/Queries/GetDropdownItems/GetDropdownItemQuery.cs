using ErrorOr;
using MediatR;
using NFCEPS.Shared.Models;

namespace NFCEPS.Application.Features.Common.Queries.GetDropdownItems;

public record GetDropdownItemQuery(string Flag) : IRequest<ErrorOr<List<DropdownListModel>>>;
