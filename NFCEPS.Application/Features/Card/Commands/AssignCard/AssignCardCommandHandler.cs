using System.Data;
using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using Npgsql;
using NFCEPS.Application.Models.Card.Response;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardCommandHandler : IRequestHandler<AssignCardCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public AssignCardCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(AssignCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Params = new
                {
                    p_flag = "A",
                    p_userid = request.UserId
                };

                var cardid = await _repo.QueryFirstOrDefaultAsync<AssignCardResponseModel>("SELECT card.fn_assign_card(@p_flag, @p_userid)", Params, CommandType.Text);
                return ApiResponse.Ok();
            }
            catch (NpgsqlException ex)
            {
                return ex.SqlState switch
                {
                    "P0001" => ApiResponse.Fail("Card has already been assigned to this user!"),
                    "P0002" => ApiResponse.Fail("Card assign failed, the user doesnot exist!"),
                    _ => ApiResponse.Fail($"Database error: {ex.Message}")
                };
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}


