using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Services;
using Billing.Host.Extensions;
using Billing.Host.Models;
using Billing.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Host.Controllers
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
        private readonly ICashTransactionService _cashTransactionService;
        private readonly IInternalHttpService _internalHttpService;

        /// <inheritdoc />
        public PublicController(ICashTransactionService cashTransactionService, IInternalHttpService internalHttpService)
        {
            _cashTransactionService = cashTransactionService;
            _internalHttpService = internalHttpService;
        }

        /// <summary>
        ///     Пополнение счета
        /// </summary>
        /// <param name="putMoneyParams"> параметры пополнения </param>
        /// <param name="cancellationToken"></param>
        /// <returns> результат пополнения со счета </returns>
        /// <response code="200"> Пополнение совершено </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPost("put-money")]
        [ProducesResponseType(typeof(MoneyResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutMoneyAsync([FromBody] PutMoneyParamsDto putMoneyParams, CancellationToken cancellationToken)
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
                
                await _cashTransactionService
                    .CreateCashTransactionAsync(putMoneyParams.ToCashTransaction(user.Id), cancellationToken);

                var totalAmount = await _cashTransactionService.GetTotalAmountAsync(user.Id, cancellationToken);
                
                return Ok(new MoneyResultDto
                {
                    OperationId = putMoneyParams.OperationId,
                    TotalAmount = totalAmount
                });
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
        ///     Проверка счета
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns> состояние счета </returns>
        /// <response code="200"> Состояние счета </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("check-money")]
        [ProducesResponseType(typeof(MoneyResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CheckMoneyAsync(CancellationToken cancellationToken)
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
                
                var totalAmount = await _cashTransactionService.GetTotalAmountAsync(user.Id, cancellationToken);
                
                return Ok(new MoneyResultDto
                {
                    TotalAmount = totalAmount
                });
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