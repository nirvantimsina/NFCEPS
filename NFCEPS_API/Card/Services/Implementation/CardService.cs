using System.Data.Common;
using NFCEPS_API.Card.Models.Request;
using NFCEPS_API.Card.Services.Interface;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Wrapper;

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
                    p_flag = "G",
                    p_userid = request.UserId,
                    p_cardid = request.CardId,
                    p_availableamount = request.AvailableAmount,
                    p_checksum = request.CheckSum
                };

                await repo.ExecuteAsync("card.sp_assigncard", Params);
                return ApiResponse.Ok();
            }

            catch (DbException ex)
            {
                if (ex.Message == "UNIQUE")
                {
                    return ApiResponse.Fail("The card already exists in the database!");
                }
                return ApiResponse.Fail("A database error has occured!");
            }
            catch
            {
                return ApiResponse.Fail("An unexpected error occurred!");
            }
        }
    }
}