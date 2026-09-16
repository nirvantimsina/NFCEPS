using ErrorOr;
using MediatR;
using Microsoft.VisualBasic;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ErrorOr<LoginResponse>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}


