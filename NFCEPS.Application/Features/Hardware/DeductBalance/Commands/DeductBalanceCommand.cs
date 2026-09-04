using MediatR;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Hardware.DeductBalance.Commands
{
    public class DeductBalanceCommand : IRequest<ApiResponse>
    {
        public int CardId { get; set; }
        public int Punch { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int EntityId { get; set; }
    }
}
