using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("Servicio/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status429TooManyRequests)]
    [Authorize]

    public class AutenticacionController : ControllerBase
    {
    }
}
