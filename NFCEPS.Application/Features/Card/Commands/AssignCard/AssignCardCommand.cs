using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardCommand : IRequest<ApiResponse>
    {
        public int UserId { get; set; }
    }
}


