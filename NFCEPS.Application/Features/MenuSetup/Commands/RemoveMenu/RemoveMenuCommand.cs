using ErrorOr;
using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.MenuSetup.Commands.RemoveMenu
{
    public class RemoveMenuCommand : IRequest<ErrorOr<StatusResponse>>
    {
        public int MenuId { get; set; }
    }
}
