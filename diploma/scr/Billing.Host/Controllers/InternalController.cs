using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Services;
using Billing.Host.Extensions;
using Billing.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Host.Controllers
{
    /// <summary>
    /// Внутренний API для взаимодействия между сервисами
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("internal")]
    [Produces("application/json")]
    public class InternalController : Controller
    {
        private readonly ICashTransactionService _cashTransactionService;

        /// <inheritdoc />
        public InternalController(ICashTransactionService cashTransactionService)
        {
            _cashTransactionService = cashTransactionService;
        }

        /// <summary>
        ///     Совершение покупки
        /// </summary>
        /// <param name="buyParams"> параметры покупки </param>
        /// <param name="cancellationToken"></param>
        /// <returns> результат снятия со счета </returns>
        /// <response code="200"> Покупка совершена </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpPost("buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> BuyAsync([FromBody] BuyParamsDto buyParams, CancellationToken cancellationToken)
        {
            try
            {
                await _cashTransactionService
                    .CreateCashTransactionAsync(buyParams.ToCashTransaction(), cancellationToken);
                
                return Ok();
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