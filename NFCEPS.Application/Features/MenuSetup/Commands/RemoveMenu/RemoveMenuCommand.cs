using MediatR;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Commands.RemoveMenu
{
    public class RemoveMenuCommand : IRequest<ApiResponse>
    {
        public int MenuId { get; set; }
    }
}
