using ErrorOr;
using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Hardware.DeductBalance.Commands
{
    public class DeductBalanceCommand : IRequest<ErrorOr<StatusResponse>>
    {
        public int CardId { get; set; }
        public int Punch { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int EntityId { get; set; }
    }
}
