using MediatR;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Commands.AddMenu
{
    public class AddMenuCommand : IRequest<ApiResponse>
    {
        public string? MenuName { get; set; }
        public int ParentId { get; set; }
        public string? Icon { get; set; }
        public string? Path { get; set; }
        public int MenuOrder { get; set; }
        public int CreatedBy { get; set; }
    }
}
