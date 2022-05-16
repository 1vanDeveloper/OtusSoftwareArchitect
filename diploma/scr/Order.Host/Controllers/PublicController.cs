using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Services;
using Order.Host.Extensions;
using Order.Host.Models;
using Order.Host.Services;

namespace Order.Host.Controllers
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
        private readonly IOrderService _orderService;

        /// <inheritdoc />
        public PublicController(IInternalHttpService internalHttpService, IOrderService orderService)
        {
            _internalHttpService = internalHttpService;
            _orderService = orderService;
        }

        /// <summary>
        ///     Формирование заказа
        /// </summary>
        /// <param name="makeOrderParams"> параметры заказа </param>
        /// <param name="cancellationToken"></param>
        /// <returns> результат формирования заказа </returns>
        /// <response code="200"> Формирование совершено </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPost("make-order")]
        [ProducesResponseType(typeof(MakeOrderResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> MakeOrderAsync([FromBody] MakeOrderParamsDto makeOrderParams, CancellationToken cancellationToken)
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

                var result = await _orderService.MakeOrderAsync(makeOrderParams.ToOrder(user.Id), cancellationToken);
                
                return Ok(result.ToMakeOrderResult());
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
        
        /// <summary>
        ///     Получение всех заказов пользователя
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns> список всех заказов пользователя </returns>
        /// <response code="200"> Список сформирован </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("get-orders")]
        [ProducesResponseType(typeof(GetOrdersResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOrdersAsync(CancellationToken cancellationToken)
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

                var result = await _orderService.GetOrdersAsync(user.Id, cancellationToken);
                
                return Ok(result.ToGetOrdersResultDto());
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