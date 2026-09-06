using ErrorOr;
using MediatR;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardCommand : IRequest<ErrorOr<AssignCardResponseModel>>
    {
        public int UserId { get; set; }
    }
}


