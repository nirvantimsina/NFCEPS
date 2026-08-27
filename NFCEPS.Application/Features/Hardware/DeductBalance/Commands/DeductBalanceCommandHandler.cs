using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using System.Data;

namespace NFCEPS.Application.Features.Hardware.DeductBalance.Commands
{
    public class DeductBalanceCommandHandler : IRequestHandler<DeductBalanceCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public DeductBalanceCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(DeductBalanceCommand request, CancellationToken cancellationToken)
        {
            var Params = new
            {
                p_cardid = request.CardId,
                p_punch = request.Punch,
                p_from = request.From,
                p_to = request.To,
                p_entityid = request.EntityId
            };
            // todo
            await _repo.ExecuteAsync("select card.fn_deduct(@p_cardid, @p_punch, @p_from, @p_to, @p_entityid)", Params, CommandType.Text);
            return ApiResponse.Ok();
        }
    }
}
