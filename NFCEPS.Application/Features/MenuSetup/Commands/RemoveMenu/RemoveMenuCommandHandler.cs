using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using System.Data;

namespace NFCEPS.Application.Features.MenuSetup.Commands.RemoveMenu
{
    public class RemoveMenuCommandHandler : IRequestHandler<RemoveMenuCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public RemoveMenuCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(RemoveMenuCommand request, CancellationToken cancellationToken)
        {
            var Params = new 
            {
                p_flag = "B",
                p_menuid = request.MenuId
            };

            var result = await _repo.QueryFirstOrDefaultAsync<StatusResponse>("SELECT * FROM sp_menusetup(@p_flag, @p_menuid)", Params, CommandType.Text);

            return ApiResponse.FromDbResult(result);
        }
    }
}
