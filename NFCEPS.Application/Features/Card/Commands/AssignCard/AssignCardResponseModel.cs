using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Card.Commands.AssignCard
{
    public class AssignCardResponseModel : StatusResponse
    {
        public int CardId { get; set; }
    }
}

