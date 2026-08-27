using ErrorOr;
using MediatR;
using NFCEPS.Application.Interfaces;
using System.Data;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardCommandHandler : IRequestHandler<AssignCardCommand, ErrorOr<AssignCardResponseModel>>
    {
        private readonly IGenericRepository _repo;

        public AssignCardCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ErrorOr<AssignCardResponseModel>> Handle(AssignCardCommand request, CancellationToken cancellationToken)
        {
            var Params = new
            {
                p_userid = request.UserId
            };

            var result = await _repo.QueryFirstOrDefaultAsync<AssignCardResponseModel>("CALL card.assign_card_by_userid(@p_userid)", Params, CommandType.Text);

            if (result == null)
            {
                return Error.NotFound(description: "No response received from the database!");
            }

            if (result.Status != "0")
            {
                return Error.Validation(code: result.Status, description: result.MSG);
            }

            return result;
        }
    }
}


