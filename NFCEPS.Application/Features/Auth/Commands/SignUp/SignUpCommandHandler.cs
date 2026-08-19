using System.Data;
using System.Data.Common;
using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Domain.Models;
using NFCEPS.Application.Helpers;

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
            try
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
            catch (DbException ex)
            {
                if (ex.Message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase) || ex.SqlState == "23505")
                {
                    return ApiResponse.Fail("Username or Phone Number already exists.");
                }
                return ApiResponse.Fail("A database error occurred during registration!");
            }
            catch
            {
                return ApiResponse.Fail("An unexpected error occurred!");
            }
        }
    }
}


