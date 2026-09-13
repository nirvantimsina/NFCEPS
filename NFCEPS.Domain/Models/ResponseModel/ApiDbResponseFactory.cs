using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Domain.Models // Keep your backend namespace intact
{
    public static class ApiResponseFactory
    {
        public static ApiResponse FromDbResult<T>(T? result) where T : StatusResponse
        {
            if (result == null) 
                return ApiResponse.Fail("No response received from the database!", "-1");

            string defaultMsg = result.Status == "0" ? "Success" : "Failed";
            
            string resolvedMessage = ErrorCodes.GetMessage(result.Status) 
                                     ?? result.MSG 
                                     ?? defaultMsg;

            return new ApiResponse
            {
                Status = result.Status,
                Message = resolvedMessage,
                Data = result
            };
        }
    }
}
