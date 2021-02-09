using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtusUserApp.Host.Models;

namespace OtusUserApp.Host.Controllers
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
        /// <summary>
        /// Проверка готовности сервиса 
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Сервис готов к работе </response>
        [HttpGet]
        [ProducesResponseType(typeof(HealthCheck), StatusCodes.Status200OK)]
        public IActionResult GetHealthCheck()
        {
            return Ok(new HealthCheck("OK"));
        }
    }
}