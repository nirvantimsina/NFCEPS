using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}


