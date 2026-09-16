using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using NFCEPS.Application.Common.Extensions;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.Application.Features.MenuSetup.Commands.AddMenu
{
    public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, ErrorOr<StatusResponse>>
    {
        private readonly IGenericRepository _repo;

        public AddMenuCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ErrorOr<StatusResponse>> Handle(AddMenuCommand request, CancellationToken cancellationToken)
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

            var result = await _repo.QueryFirstOrDefaultAsync<StatusResponse>("SELECT sp_menusetup(@p_flag, @p_menuname, @p_parentid, @p_icon, @p_path, @p_menuorder, p_createdby)", Params, CommandType.Text);
            
            return result.ToDbResult();
        }
    }
}
