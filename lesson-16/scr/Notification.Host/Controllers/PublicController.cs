using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Notification.Host.Extensions;
using Notification.Host.Models;
using Notification.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Services;
using Notification.Host.Models.Events;

namespace Notification.Host.Controllers
{
    /// <summary>
    /// Внешний API
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("public")]
    [Produces("application/json")]
    public class PublicController : Controller
    {
        private readonly IInternalHttpService _internalHttpService;
        private readonly INotificationEventService _notificationEventService;

        /// <inheritdoc />
        public PublicController(IInternalHttpService internalHttpService, INotificationEventService notificationEventService)
        {
            _internalHttpService = internalHttpService;
            _notificationEventService = notificationEventService;
        }

        /// <summary>
        ///     Получение всех сообщений по биллингу
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns> список </returns>
        /// <response code="200"> Список </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("get-billing-events")]
        [ProducesResponseType(typeof(List<BillingEvent>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBillngEventsAsync(CancellationToken cancellationToken)
        {
            var currentUserName = HttpContext.User.Claims.GetUserName();
            if (string.IsNullOrWhiteSpace(currentUserName))
            {
                return BadRequest(new ErrorDto
                {
                    Code = 400,
                    Message = "Empty user name"
                });
            }

            try
            {
                var user = await _internalHttpService.GetUserAsync(currentUserName, cancellationToken);
                if (user == null)
                {
                    return NotFound(new ErrorDto
                    {
                        Code = 404,
                        Message = $"Unrecognized user name {currentUserName}"
                    });
                }
                
                var events = await _notificationEventService.GetNotificationEventAsync(cancellationToken);
                
                return Ok(events.Select(e => new BillingEvent(e)).ToList());
            }
            catch (ArgumentException e)
            {
                return new ObjectResult(new ErrorDto { Code = 400, Message = e.Message })
                {
                    StatusCode = 400
                };
            }
            catch (Exception e)
            {
                return new ObjectResult(new ErrorDto { Code = 500, Message = e.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}