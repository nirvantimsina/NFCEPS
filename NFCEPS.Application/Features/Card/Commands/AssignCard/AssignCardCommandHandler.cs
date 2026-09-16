using ErrorOr;
using MediatR;
using NFCEPS.Application.Common.Extensions;
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
            var result = await _repo.QueryFirstOrDefaultAsync<AssignCardResponseModel>(
                "CALL card.assign_card_by_userid(@p_userid)",
                 new {p_userid = request.UserId},
                 CommandType.Text);

            return result.ToDbResult();
        }
    }
}


