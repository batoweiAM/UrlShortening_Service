using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShorteningService.Domain.Common;

namespace URLShorteningService.API.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult Problem(Error error)
        {
            var statusCode = error.Code switch
            {
                "UrlMapping.NotFound" => StatusCodes.Status404NotFound,
                "UrlMapping.Expired" => StatusCodes.Status410Gone,
                "UrlMapping.InvalidUrl" => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Message);
        }
    }
}
