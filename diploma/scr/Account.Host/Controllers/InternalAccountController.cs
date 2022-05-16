using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Domain.Services;
using Account.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Host.Controllers
{
    /// <summary>
    /// Контроллер управления пользоватеями
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("internal/account")]
    [AllowAnonymous]
    public class InternalAccountController : Controller
    {
        private readonly IUserService _userService;

        /// <inheritdoc />
        public InternalAccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Получение пользователя.
        /// </summary>
        /// <param name="userName"> логин пользователя </param>
        /// <param name="cancellationToken"></param>
        /// <returns> пользователь </returns>
        /// <response code="200"> Пользователь успешно найден </response>
        /// <response code="400"> Неверные входные данные. </response>
        /// <response code="404"> Не найден пользователь. </response>
        /// <response code="500"> Ошибка сервера. </response>
        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return NotFound(new ErrorDto
                {
                    Code = 400,
                    Message = "Empty user name"
                });
            }

            try
            {
                var dbUser = await _userService.GetUserAsync(userName, cancellationToken);
                return Ok(new UserDto(dbUser));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorDto
                {
                    Code = 404,
                    Message = $"Unrecognized user name {userName}"
                });
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