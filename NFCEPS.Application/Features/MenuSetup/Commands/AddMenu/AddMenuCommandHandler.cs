using MediatR;
using Microsoft.Extensions.Logging;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using System.Data;

namespace NFCEPS.Application.Features.MenuSetup.Commands.AddMenu
{
    public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public AddMenuCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(AddMenuCommand request, CancellationToken cancellationToken)
        {
            var Params = new
            {
                p_flag = "A",
                p_menuname = request.MenuName,
                p_parentid = request.ParentId,
                p_icon = request.Icon,
                p_path = request.Path,
                p_menuorder = request.MenuOrder,
                p_createdby = request.CreatedBy
            };

            await _repo.ExecuteAsync("SELECT sp_menusetup(@p_flag, @p_menuname, @p_parentid, @p_icon, @p_path, @p_menuorder, p_createdby)", Params, CommandType.Text);
            return ApiResponse.Ok();
        }
    }
}
