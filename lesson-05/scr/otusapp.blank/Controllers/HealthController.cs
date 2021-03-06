using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace otusapp.blank.Controllers
{
    /// <summary>
    /// Health check
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("health")]
    [Produces("application/json")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "OK"
            });
        }
    }
}