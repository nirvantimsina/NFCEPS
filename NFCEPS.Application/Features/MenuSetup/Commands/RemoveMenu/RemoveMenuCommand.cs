using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.MenuSetup.Commands.RemoveMenu
{
    public class RemoveMenuCommand : IRequest<ApiResponse>
    {
        public int MenuId { get; set; }
    }
}
