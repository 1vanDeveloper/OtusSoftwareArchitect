using Billing.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Host.Controllers
{
    /// <summary>
    /// Version check
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("")]
    [Produces("application/json")]
    public class IndexController : ControllerBase
    {
        /// <summary>
        /// Возвращает информацию о приложении
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Сервис доступен </response>
        [HttpGet]
        [ProducesResponseType(typeof(VersionDto), StatusCodes.Status200OK)]
        public IActionResult Index()
        {
            return Ok(new VersionDto
            {
                Version = "1"
            });
        }
    }
}