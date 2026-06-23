using System.Data;
using NFCEPS_API.Card.Models.Request;
using NFCEPS_API.Card.Models.Response;
using NFCEPS_API.Card.Services.Interface;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Wrapper;
using Npgsql;

namespace NFCEPS_API.Card.Services.Implementation
{
    public class CardService(IGenericRepository repo) : ICardService
    {
        public async Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request)
        {
            try
            {
                var Params = new
                {
                    p_flag = "A",
                    p_userid = request.UserId
                };

                var cardid = await repo.QueryFirstOrDefaultAsync<AssignCardResponseModel>("SELECT card.fn_assign_card(@p_flag, @p_userid)", Params, CommandType.Text);
                return ApiResponse.Ok();
            }

            catch (NpgsqlException ex)
            {
                return ex.SqlState switch
                {
                    "P0001" => ApiResponse.Fail("Card has already been assigned to this user!"),
                    "P0002" => ApiResponse.Fail("Card assign failed, the user doesnot exist!"),
                    _ => ApiResponse.Fail($"Database error: {ex.Message}")
                };
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}