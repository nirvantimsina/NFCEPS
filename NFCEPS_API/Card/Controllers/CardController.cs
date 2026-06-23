using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Card.Models.Request;
using NFCEPS_API.Card.Services.Interface;
using NFCEPS_API.Controllers;

namespace NFCEPS_API.Card.Controllers
{
    public class CardController(ICardService cardService) : ApiBaseController
    {
        [HttpPost("AssignCard")]
        public async Task<IActionResult> AssignCard([FromBody] AssignCardRequestModel request)
        {
            var result = await cardService.AssignCardAsync(request);
            return HandleResponse(result);
        }
    }
}