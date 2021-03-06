using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OtusUserApp.Host.Models;

namespace OtusUserApp.Host.Controllers
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
        /// <summary>
        /// Возвращает информацию о приложении
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Сервис доступен </response>
        [HttpGet]
        [ProducesResponseType(typeof(VersionDto), StatusCodes.Status200OK)]
        public IActionResult Index()
        {
            return Ok(new VersionDto("3"));
        }
    }
}
