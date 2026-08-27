using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using System.Data;

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
                    p_userid = request.UserId
                };

                var result = await _repo.QueryFirstOrDefaultAsync<AssignCardResponseModel>("CALL card.assign_card_by_userid(@p_userid)", Params, CommandType.Text);

                return ApiResponse.FromDbResult(result);
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}


