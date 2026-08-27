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
            try
            {
                var Params = new 
                {
                    p_flag = "B",
                    p_menuid = request.MenuId
                };

                var result = await _repo.QueryFirstOrDefaultAsync<StatusResponse>("SELECT * FROM sp_menusetup(@p_flag, @p_menuid)", Params, CommandType.Text);

                if (result == null)
                {
                    return ApiResponse.Fail("Database did not return a status response.");
                }

                if (result.Status == 0)
                {
                    return ApiResponse.Ok(result.MSG);
                }
                else if (result.Status == 1)
                {
                    return ApiResponse.Ok(result);
                }
            }

            catch (Exception ex)
            {
                return ApiResponse.Fail($"An error occured! {ex.Message}");
            }

            return ApiResponse.Fail("An unexpected code execution path occurred.");
        }
    }
}
