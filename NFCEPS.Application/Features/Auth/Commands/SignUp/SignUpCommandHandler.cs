using MediatR;
using NFCEPS.Application.Helpers;
using NFCEPS.Application.Interfaces;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.Application.Features.Auth.Commands.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public SignUpCommandHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = PasswordHelper.HashPassword(request.Password);
            var signUpParams = new
            {
                p_flag = "A",
                p_username = request.UserName,
                p_name = request.Name,
                p_address = request.Address,
                p_phone = request.Phone,
                p_password = hashedPassword
            };

            await _repo.ExecuteAsync(
                "SELECT permission.fn_auth(@p_flag, @p_username, @p_name, @p_address, @p_phone, @p_password)",
                signUpParams,
                commandType: CommandType.Text);

            return ApiResponse.Ok();
        }
    }
}


