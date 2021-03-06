using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace otusapp.blank.Controllers
{
    /// <summary>
    /// Health check
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("")]
    [Produces("application/json")]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                version = "2"
            });
        }
    }
}
